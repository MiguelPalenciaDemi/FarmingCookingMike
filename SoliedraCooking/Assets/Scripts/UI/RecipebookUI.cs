using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RecipebookUI : MonoBehaviour
{
    private static RecipebookUI _instance;
    public static RecipebookUI Instance => _instance;
    
    [SerializeField] private Transform contentList;
    [SerializeField] private GameObject recipeTitleUIPrefab;

    [SerializeField] private RecipeInfoUI recipeInfoUI;
    
    [SerializeField] private Animator _animator;
    private bool _isShown;
    private RecipeTitleUI _currentRecipeSelected;
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
        var recipeBook = OrderManager.Instance.GetCurrentRecipeBook();
        RemoveChildren();
        foreach (var recipe in recipeBook.Recipes)
        {
            AddRecipe(recipe);
        }
    }
    public void AddRecipe(Recipe recipe)
    {
        var recipeUI = Instantiate(recipeTitleUIPrefab, contentList).GetComponent<RecipeTitleUI>();
        recipeUI.SetUp(recipe);
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
        recipeInfoUI.Setup(recipeSelected.Recipe);
    }

    public void ShowRecipeBook()
    {
        _isShown = !_isShown;
        _animator.SetBool("IsShown",_isShown);
    }

}
