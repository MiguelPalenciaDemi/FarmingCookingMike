using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private float interactRange;
    [SerializeField] private Transform handPos;
    [SerializeField] private GameObject knife;
    [SerializeField] private GameObject rollingPin;

    [Header("Audio")]
    [SerializeField] private EventReference takeAudio;
    [SerializeField] private EventReference dropAudio;
    private IInteractable _objectInteractable;
    private ITakeDrop _objectPickable;
    
    private bool _canInteract;
    private bool _canTakeDrop;
    
    private GameObject _objectPickedUp;
    private Animator _animator;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }


    public GameObject ObjectPickedUp => _objectPickedUp;

    private void Update()
    {
        CheckInteract();
        
        //Mostrar UI de poder interactuar
        CheckPrompts();
        
    }

    public void TakeDropInput()
    {
        if(_canTakeDrop)
            _objectPickable.TakeDrop(this);
    }

    public void InteractInput()
    {
        if(_canInteract)
            _objectInteractable.Interact(this);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void CheckInteract()
    {
        
        if (!Physics.Raycast(transform.position+Vector3.up/2, transform.forward, out var hit, interactRange, interactLayer))//+Vector3.up/2 to avoid ground collision
        {
            _canInteract = false;
            _canTakeDrop = false;
            
            _objectInteractable = null;
            _objectPickable = null;
            return;
        }

        if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
        {
            _canInteract = true;
            _objectInteractable = interactable;
        }
        
        if(hit.collider.TryGetComponent<ITakeDrop>(out var pickable ))
        {
            _canTakeDrop = true;
            _objectPickable = pickable;
        }
        
            
        
    }

    public void TakeObject(GameObject objectPicked)
    {
        _objectPickedUp = objectPicked;
        _objectPickedUp.transform.parent = handPos;
        _objectPickedUp.transform.localPosition = Vector3.zero;
        _animator.SetBool("Carry", true);
        AudioManager.Instance.PlaySoundAtPosition(takeAudio,transform);

        //_objectPickedUp.transform.localRotation = Quaternion.identity;
    }
    
    public GameObject DropObject()
    {
        var dropObject = _objectPickedUp;
        _objectPickedUp = null;
        _animator.SetBool("Carry", false);
        AudioManager.Instance.PlaySoundAtPosition(dropAudio,transform);
        return dropObject;
    }

    public void ChopAnimation()
    {
        Debug.Log("heyy chop");
        _animator.SetTrigger("Chop");
        ShowKnife(true);
    }

    private void ShowKnife(bool value)
    {
        knife.SetActive(value);
    }

    public void HideKnife()
    {
        ShowKnife(false);
        ShowRollingPin(false);
    }
    
    public void SmashAnimation()
    {
        _animator.SetTrigger("Chop");
        ShowRollingPin(true);
    }

    private void ShowRollingPin(bool value)
    {
        rollingPin.SetActive(value);
    }

    private void CheckPrompts()
    {
        InputPromptsManager.Instance.SetActiveInteractPrompt(_objectInteractable != null && _objectInteractable.CanInteract());
        
        //TakeCondition
        var handFreeOtherBusy = !_objectPickedUp && _objectPickable != null && _objectPickable.CanTakeDrop(); //Coger
        var handBusyOtherFree = _objectPickedUp && _objectPickable != null && !_objectPickable.CanTakeDrop(); //Dejar
        var handPlateOtherBusy = (_objectPickedUp && _objectPickedUp.TryGetComponent(out Plate plate)) &&
                             (_objectPickable != null && _objectPickable.CanTakeDrop()); // Tenemos un plato en la mano y en la encimera algo
        var workstation = _objectPickable as Workstation;
        ////Tienes un plato en la mano y enfrente tienes un worktation
        var handIngredientOtherBusy = (_objectPickedUp && _objectPickedUp.TryGetComponent(out Ingredient ingredient)) &&
                                  (workstation && workstation.ObjectInWorktop && workstation.ObjectInWorktop.GetComponent<Plate>());
                                      
        InputPromptsManager.Instance.SetActiveTakeDropPrompt(handBusyOtherFree || handFreeOtherBusy || handPlateOtherBusy || handIngredientOtherBusy);
    }

    
}
