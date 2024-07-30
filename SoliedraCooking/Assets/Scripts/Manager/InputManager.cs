using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance => _instance;
    
    [SerializeField] private GameObject player;
    private PlayerMovement _playerMovement;
    private PlayerInteract _playerInteract;
    private PlayerInput _playerInput;
    private string _currentScheme;
    private void Awake()
    {
        //GetScripts from Player
        player.TryGetComponent(out _playerInteract);
        player.TryGetComponent(out _playerMovement);
        _playerInput = GetComponent<PlayerInput>();
        
        
    }

    private void Update()
    {
        CheckCurrentScheme();
    }

    private void CheckCurrentScheme()
    {
        if(_playerInput.currentControlScheme != null && _currentScheme == _playerInput.currentControlScheme) return;

        _currentScheme = _playerInput.currentControlScheme;
        var mode = _currentScheme == "Keyboard" ? InputMode.Keyboard : InputMode.Gamepad; 
        InputPromptsManager.Instance.SetInputMode(mode);
    }

    private void OnControlChanged(InputUser arg1, InputUserChange arg2, InputDevice arg3)
    {
        Debug.Log(_playerInput.currentControlScheme.ToString());
    }

    public void OnMove(InputValue value)
    {
        _playerMovement.MoveInput(value.Get<Vector2>());
    }

    public void OnInteract(InputValue value)
    {
        _playerInteract.InteractInput();
    }

    public void OnTakeDrop()
    {
        _playerInteract.TakeDropInput();
    }

    public void OnShowRecipeBook()
    {
        RecipebookUI.Instance.ShowRecipeBook();
    }

    public void OnControlChanged(PlayerInput input)
    {
        Debug.Log(_playerInput.currentControlScheme);

    }
}
