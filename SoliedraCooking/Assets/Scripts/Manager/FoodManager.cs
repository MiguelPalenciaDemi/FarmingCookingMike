using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    private static FoodManager _instance;
    public static FoodManager Instance => _instance;

    [SerializeField] private ListOfFoodIcons foodIcons;
    [SerializeField] private ListOfFoodModels models;
    [SerializeField] private ListOfFoodModels modelsPot;
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
    public bool CheckPotRecipe(List<IngredientInfo> ingredients)
    {
        return potRecipes.SearchMatchRecipe(ingredients);
    }
    
    //Comprueba si corresponde con el proceso de alguna receta de olla
    public bool CheckPlateIngredients(List<IngredientInfo> ingredients)
    {
        return plateRecipes.ValidIngredients(ingredients);
    }

    public GameObject GetFoodModel(List<IngredientInfo> ingredients)
    {
        return models.GetModel(ingredients);
    }
    
    public GameObject GetFoodModelPot(List<IngredientInfo> ingredients)
    {
        return modelsPot.GetModel(ingredients);
    }

    public Sprite GetFoodIcon(FoodTag foodTag)
    {
        Debug.Log(foodTag.ToString());
        return foodIcons.Icons.Find(x => x.Tag == foodTag).Icon;
    }

}
