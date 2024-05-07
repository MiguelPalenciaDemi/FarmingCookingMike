using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private float interactRange;
    [SerializeField] private Transform handPos;
    
    private IInteractable _objectInteractable;
    private ITakeDrop _objectPickable;
    
    private bool _canInteract;
    private bool _canTakeDrop;
    
    private GameObject _objectPickedUp;
    
    

    public GameObject ObjectPickedUp => _objectPickedUp;

    private void Update()
    {
        CheckInteract();
        
        // if (_canInteract && Input.GetButtonDown("Interact")) 
        // {
        //     _objectInteractable.Interact(this);
        // }
        //
        // if (_canTakeDrop && Input.GetButtonDown("TakeDrop"))
        // {
        //     _objectPickable.TakeDrop(this);
        // }
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
            // if(_objectInteractable != null) //Si antes podiamos interactuar con algo, desactivamos su UI
            //     _objectInteractable.ShowUI(false);
            //
            _objectInteractable = null;
            _objectPickable = null;
            return;
        }

        if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
        {
            _canInteract = true;
            _objectInteractable = interactable;
           // _objectInteractable.ShowUI(true);
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
        _objectPickedUp.transform.localRotation = Quaternion.identity;
    }
    
    public GameObject DropObject()
    {
        var dropObject = _objectPickedUp;
        _objectPickedUp = null;
        return dropObject;
    }

    
}
