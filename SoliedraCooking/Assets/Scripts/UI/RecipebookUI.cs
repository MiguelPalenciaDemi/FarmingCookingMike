using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class RecipebookUI : MonoBehaviour
{
    private static RecipebookUI _instance;
    public static RecipebookUI Instance => _instance;
    
    [SerializeField] private Transform contentList;
    [SerializeField] private GameObject recipeTitleUIPrefab;
    [SerializeField] private RecipeInfoUI recipeInfoUI;
    [SerializeField] private Animator animator;
    
    private RecipeTitleUI _currentRecipeSelected;
    private bool _isShown;
    private List<RecipeTitleUI> _listRecipe;
    private int _currentIndex;
    
    private void Awake()
    {
        if (_instance)
            Destroy(this);
        else
            _instance = this;

        
    }

    public void Start()
    {
        Init();
    }

    private void Init()
    {
        _listRecipe = new List<RecipeTitleUI>();
        var recipeBook = OrderManager.Instance.GetCurrentRecipeBook();
        RemoveChildren();
        foreach (var recipe in recipeBook.Recipes)
        {
            AddRecipe(recipe);
        }

        if (_listRecipe.Count > 0)
        {
            _listRecipe[0].Select();
        }
        
    }
    public void AddRecipe(Recipe recipe)
    {
        var recipeUI = Instantiate(recipeTitleUIPrefab, contentList).GetComponent<RecipeTitleUI>();
        recipeUI.SetUp(recipe);
        _listRecipe.Add(recipeUI);
    }
    
    public void RemoveChildren()
    {
        //Remove model
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void ShowSelectRecipeItem(RecipeTitleUI recipeSelected)
    {
        //Deseleccionamos la anterior receta seleccionada
        if(_currentRecipeSelected)
            _currentRecipeSelected.ShowBorder(false);
        
        //Seleccionamos y mostramos la nueva receta
        _currentRecipeSelected = recipeSelected;
        _currentIndex = _listRecipe.FindIndex(x => x == _currentRecipeSelected);
        recipeInfoUI.Setup(recipeSelected.Recipe);
    }

    public void ShowRecipeBook()
    {
        _isShown = !_isShown;
        animator.SetBool("IsShown",_isShown);
        EventSystem.current.firstSelectedGameObject = GetCurrentRecipeGameObject();
    }

    private GameObject GetCurrentRecipeGameObject()
    {
        return _listRecipe.Find(x =>x == _currentRecipeSelected).gameObject;
    }

    public void Navigate(float value)
    {
        var sumIndex = value > 0 ? 1 : -1;

        if (_currentIndex + sumIndex >= _listRecipe.Count)
            _currentIndex = 0;
        else if (_currentIndex + sumIndex < 0)
        {
            _currentIndex = _listRecipe.Count - 1;
        }
        else
        {
            _currentIndex += sumIndex;
        }
        
        //Deseleccionamos la anterior receta seleccionada
        if(_currentRecipeSelected)
            _currentRecipeSelected.ShowBorder(false);
        
        //Seleccionamos y mostramos la nueva receta
        _listRecipe[_currentIndex].Select();
        //_currentRecipeSelected.Select();
        // _currentRecipeSelected.ShowBorder(true);
        // recipeInfoUI.Setup(_currentRecipeSelected.Recipe);
        

    }
}
