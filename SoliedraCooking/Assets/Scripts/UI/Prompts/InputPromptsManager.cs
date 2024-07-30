using UnityEngine;


public class InputPromptsManager : MonoBehaviour
{
    private static InputPromptsManager _instance;
    public static InputPromptsManager Instance => _instance;
    
    [SerializeField] private InputPrompt interactPrompt;
    [SerializeField] private InputPrompt takeDropPrompt;

    private InputMode _currentInputMode = InputMode.None;
    private void Awake()
    {
        if (_instance)
            Destroy(this);
        else
            _instance = this;
    }

    public void SetActiveInteractPrompt(bool value)
    {
        interactPrompt.gameObject.SetActive(value);
    }
    
    public void SetActiveTakeDropPrompt(bool value)
    {
        takeDropPrompt.gameObject.SetActive(value);
    }

    public void SetInputMode(InputMode mode)
    {
        if(_currentInputMode == mode) return;

        _currentInputMode = mode;
        interactPrompt.SetIcon(_currentInputMode);
        takeDropPrompt.SetIcon(_currentInputMode);
    }
    
}
