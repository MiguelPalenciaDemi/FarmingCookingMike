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
        if (_instance)
            Destroy(this);
        else
            _instance = this;
        
        //GetScripts from Player
        _playerInput = GetComponent<PlayerInput>();
        if (player)
        {
            player.TryGetComponent(out _playerInteract);
            player.TryGetComponent(out _playerMovement);
        }
        
        
        
    }

    private void Update()
    {
        CheckCurrentScheme();
    }

    private void CheckCurrentScheme()
    {
        var promptManager = InputPromptsManager.Instance;
        if(!promptManager || _playerInput.currentControlScheme != null && _currentScheme == _playerInput.currentControlScheme) return;

        _currentScheme = _playerInput.currentControlScheme;
        var mode = _currentScheme == "Keyboard" ? InputMode.Keyboard : InputMode.Gamepad; 
        promptManager.SetInputMode(mode);
    }

    public void ActiveControl(bool value)
    {
        if(value)
            _playerInput.ActivateInput();
        else 
            _playerInput.DeactivateInput();
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
        if(_playerInput.currentActionMap.name == "MenuNavigation") return;
        
        _onMenu = !_onMenu;
        _playerInput.SwitchCurrentActionMap(_onMenu ? "UI" : "Player");

        RecipebookUI.Instance.ShowRecipeBook();
    }

    #region MenuNavigation

    public void OnNavigateMenu(InputValue value)
    {
        MenuManager.Instance.Navigate(value);
    }

    public void OnSelect()
    {
        MenuManager.Instance.Select();
    }

    public void OnShowMenu()
    {
        if(_playerInput.currentActionMap.name == "UI" && MenuManager.Instance.IsThereMenuOptions()) return;
        
        _onMenu = !_onMenu;
        _playerInput.SwitchCurrentActionMap(_onMenu ? "MenuNavigation" : "Player");
        
        MenuManager.Instance.ShowMenu();

    }

    public void SetMenuNavigable(bool value)
    {
        _playerInput.SwitchCurrentActionMap(value ? "MenuNavigation" : "Player");

    }

    #endregion

   
}
