using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerMovement : MonoBehaviour
{
    
    
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 150f;
    [SerializeField] private float runSpeed = 200f;
    private float _speedMovement = 5f;
    

    [Header("Rotation")]
    [SerializeField] private float turnRate = 5f;
    
    [Header("Audio")] 
    [SerializeField] private EventReference footstepSound;
    
    private Rigidbody _rb;
    private Animator _anim;

    
    private Vector3 _movement;
    private float _horizontal;
    private float _vertical;

    
    // Start is called before the first frame update
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _speedMovement = walkSpeed;
    }

    private void Update()
    {
        Animating();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }


    public void MoveInput(Vector2 inputMovement)
    {
        
        _horizontal = inputMovement.x;
        _vertical = inputMovement.y;
        
        _movement = new Vector3(_horizontal, 0, _vertical);
        _movement.Normalize();
        
    }
    
   
    
    private void Move()
    {
        var horizontalVelocity = new Vector2(_movement.x,  _movement.z) * (_speedMovement * Time.deltaTime);
        _rb.velocity = new Vector3(horizontalVelocity.x, _rb.velocity.y, horizontalVelocity.y);

    }

    private void Rotate()
    {
        var forward = Vector3.RotateTowards(transform.forward, _movement, turnRate * Time.deltaTime,0);
        var newRotation = Quaternion.LookRotation(forward, Vector3.up);
        _rb.MoveRotation(newRotation);
    }

    private void Animating()
    {
        _anim.SetFloat("Velocity",_rb.velocity.magnitude);
    }

    public void PlayFootstep()
    {
        AudioManager.Instance.PlaySoundAtPosition(footstepSound,transform);
    }
    
}


