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
    private bool _onRecipeBook;
    private bool _onPause;
    private string _lastActionMap;
    private string _currentActionMap;
    
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
        if(!_onRecipeBook)
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

    public void OnNavigate(InputValue value)
    {
        Debug.Log("a navegar");
        RecipebookUI.Instance.Navigate(value.Get<float>());
    }

    public void OnShowRecipeBook()
    {
        if(_onPause) return;
        
        _onRecipeBook = !_onRecipeBook; //Necesario para cambiar entre receta y juego, distinto a la pausa.
        _playerInput.SwitchCurrentActionMap(_onRecipeBook ? "UI" : "Player");

        RecipebookUI.Instance.ShowRecipeBook();
    }

    public void OnPauseGame()
    {
        if (!MenuManager.Instance.IsThereMenuOptions()) return; //En caso de que no haya menu de pausa/opciones, salimos.

        if (!_onPause)
        {
            _lastActionMap = _playerInput.currentActionMap.name; //Solamente se cambia si entramos en pausa, sino se guardaria el MenuNavigation que no es necesario
            _playerInput.SwitchCurrentActionMap("MenuNavigation"); //Cambia el action map
            
            _onPause = true;
            MenuManager.Instance.ShowPauseMenu();
        }
        else if (_onPause)
        {
            MenuManager.Instance.CloseMenu();
            
            if (!MenuManager.Instance.IsThereCurrentMenu()) //Comprobamos 
            {
                _playerInput.SwitchCurrentActionMap(_lastActionMap); //Cambia el action map
                _onPause = false;
            }
            
        }
       
        
        
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

    

    public void SetMenuNavigable(bool value)
    {
        _playerInput.SwitchCurrentActionMap(value ? "MenuNavigation" : "Player");

    }

    public void OnPress()
    {
        MenuManager.Instance.Press();
    }
    
    public void OnInteractUI(InputValue value)
    {
        MenuManager.Instance.Interact(value);
    }

    #endregion

   
}
