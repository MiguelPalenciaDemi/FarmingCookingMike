using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    private static FoodManager _instance;
    public static FoodManager Instance => _instance;

    [SerializeField] private RecipeBook potRecipes;
    [SerializeField] private RecipeBook plateRecipes;
    private void Awake()
    {
        if (_instance)
            Destroy(this);
        else
            _instance = this;
    }

    //Comprueba si corresponde con el proceso de alguna receta de olla
    public bool CheckPotIngredients(List<IngredientInfo> ingredients)
    {
        return potRecipes.ValidIngredients(ingredients);
    }
    
    //Comprueba si corresponde con el proceso de alguna receta de olla
    public bool CheckPlateIngredients(List<IngredientInfo> ingredients)
    {
        return plateRecipes.ValidIngredients(ingredients);
    }

}
