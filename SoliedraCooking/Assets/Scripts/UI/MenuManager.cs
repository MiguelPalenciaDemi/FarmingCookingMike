using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MenuManager : MonoBehaviour
{
    private static MenuManager _instance;
    public static MenuManager Instance => _instance;

    [SerializeField] private InitialTransitionController transitionController;
    [SerializeField] private List<MenuNavigation> menus;
    [SerializeField] private GameObject menuPause;
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
        if (!newMenu) return;
        
        _currentMenu = newMenu;
        _currentMenu.gameObject.SetActive(true);


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

    public void ShowPauseMenu()
    {
        if (!menuPause) return;

        menuPause.SetActive(!menuPause.activeInHierarchy);
        
        if (menuPause.activeInHierarchy)
            SetCurrentMenu(menuPause.GetComponent<MenuNavigation>());
    }

    public bool IsThereMenuOptions()
    {
        return menuPause;
    }

    public void ShowStartMenu(bool value)
    {
        InputManager.Instance.SetMenuNavigable(value);
        startMenu.SetActive(value);
        
        //En caso de que se vaya a enseñar
        if (value)
            SetCurrentMenu(startMenu.GetComponent<MenuNavigation>());
        
        
    }

    public void ShowEndMenu(bool value)
    {
        InputManager.Instance.SetMenuNavigable(value);

        endMenu.SetActive(value);
        //En caso de que se vaya a enseñar
        if (value)
            SetCurrentMenu(endMenu.GetComponent<MenuNavigation>());
    }

    public void ExitToTitleScreen()
    {
        StartCoroutine(transitionController.EndWithAction( () =>{ SceneManager.LoadScene("StartScene"); }));
        
    }

    public void Press()
    {
        _currentMenu.Press();
    }
    
    public void Interact(InputValue value)
    {
        var direction = value.Get<float>();
        direction = direction > 0 ? 1 : -1;
        _currentMenu.Interact((int) direction);
    }
}
