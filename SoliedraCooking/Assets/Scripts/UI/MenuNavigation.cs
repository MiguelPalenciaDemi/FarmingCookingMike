using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class MenuNavigation : MonoBehaviour
{
    [SerializeField] private MenuNavigation backMenuNavigation;
    [SerializeField] private List<GameObject> navigableGameObjects;
    private List<INavigableUI> navigableUIList;
    private int _currentIndex;

    

    private void Awake()
    {
        InitializeNavigable();
        ResetButtons();
    }

    private void InitializeNavigable()
    {
        navigableUIList = new List<INavigableUI>();
        foreach (var button in navigableGameObjects)
        {
            var navigableInterface = button.GetComponent<INavigableUI>();
            if(navigableInterface != null)
                navigableUIList.Add(navigableInterface);
        }
        
    }

    private void OnEnable()
    {
        if (navigableUIList == null) InitializeNavigable();
        ResetButtons();
        
    }
    

    public void ResetButtons()
    {   
        if(navigableUIList.Count == 0) return;

        foreach (var button in navigableUIList)
        {
            button.Select(false);
        }

        navigableUIList[0].Select(true);
        _currentIndex = 0;

    }
    public void Navigate(int direction)
    {
        Debug.Log(direction);
        navigableUIList[_currentIndex].Select(false);

        if (_currentIndex + direction >= navigableUIList.Count)
            _currentIndex = 0;
        else if (_currentIndex + direction < 0)
            _currentIndex = navigableUIList.Count - 1;
        else
            _currentIndex += direction;

        navigableUIList[_currentIndex].Select(true);

    }

    public void Select()
    {
        navigableUIList[_currentIndex].Interact();
    }

    public void Press()
    {
        navigableUIList[_currentIndex].Press();
    }

    public void Interact(int value)
    {
        navigableUIList[_currentIndex].Interact(value);
    }

    public void BackMenu()
    {
        gameObject.SetActive(false);
        MenuManager.Instance.SetCurrentMenu(backMenuNavigation);
    }

    public bool HasBackMenu() => backMenuNavigation;

    public void OpenMenu()
    {
        gameObject.SetActive(true);
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }
}
