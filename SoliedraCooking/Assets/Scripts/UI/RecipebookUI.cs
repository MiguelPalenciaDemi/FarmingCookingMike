using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipebookUI : MonoBehaviour
{
    [SerializeField] private Transform contentList;
    [SerializeField] private GameObject recipeTitleUIPrefab;

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
        recipeUI.SetUp(recipe.RecipeImage,recipe.name);
    }
    
    public void RemoveChildren()
    {
        //Remove model
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
    
}
