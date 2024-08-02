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
    private bool _onMenu;
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
    

    public void OnMove(InputValue value)
    {
        if(!_onMenu)
            _playerMovement.MoveInput(value.Get<Vector2>());
    }

    public void OnInteract(InputValue value)
    {
        if(!_onMenu)
            _playerInteract.InteractInput();
    }

    public void OnTakeDrop()
    {
        if(!_onMenu)
            _playerInteract.TakeDropInput();
    }

    public void OnNavigate(InputValue value)
    {
        
        Debug.Log("a navegar");
        RecipebookUI.Instance.Navigate(value.Get<float>());
    }

    public void OnShowRecipeBook()
    {
        _onMenu = !_onMenu;
        _playerInput.SwitchCurrentActionMap(_onMenu ? "UI" : "Player");

        RecipebookUI.Instance.ShowRecipeBook();
    }

    
}
