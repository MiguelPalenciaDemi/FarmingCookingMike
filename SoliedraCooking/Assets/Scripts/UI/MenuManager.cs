using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    private static MenuManager _instance;
    public static MenuManager Instance => _instance;

    [SerializeField] private List<MenuNavigation> menus;
    [SerializeField] private GameObject menuOptions;
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject endMenu;
    [SerializeField] private MenuNavigation _currentMenu;
    private void Awake()
    {
        if (_instance)
            Destroy(this);
        else
            _instance = this;
    }

    private void Start()
    {
        if(startMenu)
            ShowStartMenu(true);
    }

    public void SetCurrentMenu(MenuNavigation newMenu)
    {
        if (newMenu)
            _currentMenu = newMenu;
    }
    
    public void Navigate(InputValue value)
    {
        var direction = value.Get<float>();
        direction = direction > 0 ? 1 : -1;
        
        _currentMenu.Navigate((int)direction);

    }
    
    public void Select()
    {
        _currentMenu.Select();
    }

    public void ShowMenu()
    {
        if (!menuOptions) return;
        
        _currentMenu = menuOptions.GetComponent<MenuNavigation>();
        menuOptions.SetActive(!menuOptions.activeInHierarchy);
    }

    public bool IsThereMenuOptions()
    {
        return menuOptions;
    }

    public void ShowStartMenu(bool value)
    {
        InputManager.Instance.SetMenuNavigable(value);
        _currentMenu = startMenu.GetComponent<MenuNavigation>();
        startMenu.SetActive(value);
    }

    public void ShowEndMenu(bool value)
    {
        InputManager.Instance.SetMenuNavigable(value);

        _currentMenu = endMenu.GetComponent<MenuNavigation>();
        endMenu.SetActive(value);
    }
}
