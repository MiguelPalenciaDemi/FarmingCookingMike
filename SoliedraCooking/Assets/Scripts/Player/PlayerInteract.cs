using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private float interactRange;
    [SerializeField] private Transform handPos;
    [SerializeField] private GameObject knife;
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
        
            
        //Mostrar UI de poder interactuar

    }

    public void TakeObject(GameObject objectPicked)
    {
        _objectPickedUp = objectPicked;
        _objectPickedUp.transform.parent = handPos;
        _objectPickedUp.transform.localPosition = Vector3.zero;
        _animator.SetBool("Carry", true);

        //_objectPickedUp.transform.localRotation = Quaternion.identity;
    }
    
    public GameObject DropObject()
    {
        var dropObject = _objectPickedUp;
        _objectPickedUp = null;
        _animator.SetBool("Carry", false);

        return dropObject;
    }

    public void ChopAnimation()
    {
        Debug.Log("heyy chop");
        _animator.SetTrigger("Chop");
        knife.SetActive(true);
    }

    public void ShowKnife(bool value)
    {
        knife.SetActive(value);
    }

    public void HideKnife()
    {
        ShowKnife(false);
    }

    
}
