
using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct InteractIcon
{
    public GameObject icon;
    public CookAction action;
}

[Serializable]
public struct FoodModel
{
    [SerializeField] private List<IngredientInfo> _ingredients;
    [SerializeField] private GameObject model;

    public GameObject Model => model;

    public bool MatchIngredients(List<IngredientInfo> otherIngredients)
    {
        if (_ingredients.Count != otherIngredients.Count) return false;

        var tmpList = new List<IngredientInfo>(_ingredients);
        foreach (var ingredient in otherIngredients)
        {
           var index = tmpList.FindIndex(x => x == ingredient);
           if (index == -1) return false;
           
           tmpList.RemoveAt(index);
        }

        return tmpList.Count == 0;

    }

}


public enum FoodTag{Meat,Cheese, Lettuce, Tomato, Bread, Flour, Potato, Steak, Mushroom, Onion, Water}
public enum  IngredientState
{
    Raw, MediumRare ,Cooked, Overcooked, Chopped, Smashed, None
}

public enum CookAction
{
    Cook,Smash,Chop,None
}
