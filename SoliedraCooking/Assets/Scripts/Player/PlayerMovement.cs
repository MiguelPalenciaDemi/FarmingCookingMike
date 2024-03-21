using System;
using UnityEngine;
using UnityEngine.Serialization;


public class PlayerMovement : MonoBehaviour
{
    
    
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 150f;
    [SerializeField] private float runSpeed = 200f;
    private float _speedMovement = 5f;
    

    [Header("Rotation")]
    [SerializeField] private float turnRate = 5f;

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
        PlayerInput();
        Animating();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }


    private void PlayerInput()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");
        _movement = new Vector3(_horizontal, 0, _vertical);
        _movement.Normalize();
        

        //_speedMovement = Input.GetButton("Run") ? runSpeed : walkSpeed; POr ahora solo andamos rapido
        
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
}


