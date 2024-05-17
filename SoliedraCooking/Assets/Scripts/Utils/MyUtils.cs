
using System;
using UnityEngine;


[Serializable]
public struct InteractIcon
{
    public GameObject icon;
    public CookAction action;
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
