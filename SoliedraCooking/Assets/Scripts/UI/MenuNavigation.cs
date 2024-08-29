using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class MenuNavigation : MonoBehaviour
{
    [SerializeField] private List<MyButton> buttons;
    private int _currentIndex;

    private void Start()
    {
        _currentIndex = 0;
        if(buttons.Count>0)
            buttons[_currentIndex].Select(true);
    }

    public void Navigate(int direction)
    {
        Debug.Log(direction);
        buttons[_currentIndex].Select(false);

        if (_currentIndex + direction >= buttons.Count)
            _currentIndex = 0;
        else if (_currentIndex + direction < 0)
            _currentIndex = buttons.Count - 1;
        else
            _currentIndex += direction;

        buttons[_currentIndex].Select(true);

    }

    public void Select()
    {
        Debug.Log("he sido pulsado");
        buttons[_currentIndex].onClick.Invoke();
    }
}
