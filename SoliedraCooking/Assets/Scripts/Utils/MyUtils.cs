
using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public struct InteractIcon
{
    public GameObject icon;
    [FormerlySerializedAs("action")] public CookAction action;
}


[System.Serializable]
public struct IngredientRecipeStruct
{
    [SerializeField] private FoodTag ingredient;
    [SerializeField] private IngredientState state;
    
    
    public FoodTag Ingredient => ingredient;
    public IngredientState State => state;

    

    public IngredientRecipeStruct(FoodTag ingredient, IngredientState state)
    {
        this.ingredient = ingredient;
        this.state = state;
    }
    public bool Compare(FoodTag otherIngredient, IngredientState otherState)
    {
        return ingredient == otherIngredient && state == otherState;
    }
    public bool Compare(IngredientRecipeStruct other)
    {
        return ingredient == other.ingredient && state == other.state;
    }
    
}

public enum FoodTag{Ham,Cheese, Lettuce, Tomato, Bread, Flour}
public enum  IngredientState
{
    Raw, MediumRare ,Cooked, Overcooked, Chopped, Smashed, None
}

public enum CookAction
{
    Cook,Smash,Chop,None
}
