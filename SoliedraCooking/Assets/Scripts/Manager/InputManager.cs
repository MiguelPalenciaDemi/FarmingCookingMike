using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance => _instance;
    
    [SerializeField] private GameObject player;
    private PlayerMovement _playerMovement;
    private PlayerInteract _playerInteract;

    private void Awake()
    {
        //GetScripts from Player
        player.TryGetComponent(out _playerInteract);
        player.TryGetComponent(out _playerMovement);
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
}
