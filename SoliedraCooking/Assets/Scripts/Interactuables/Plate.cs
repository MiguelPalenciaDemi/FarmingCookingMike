using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField] private Transform modelParent;
    private List<IngredientInfo> _ingredients;

    public List<IngredientInfo> Ingredients => _ingredients;

    private void Awake()
    {
        _ingredients = new List<IngredientInfo>();
    }

    public bool AddIngredient(IngredientInfo newIngredient)
    {
        if (!CanAddIngredient(newIngredient)) return false;
        
        _ingredients.Add(newIngredient);
        return true;

        //Change Model depend on ingredients in plate
    }

    public void Clean()
    {
        _ingredients.Clear();
        
        //Remove model
        foreach (Transform model in modelParent)
        {
            Destroy(model.gameObject);
        }
    }
    
    private bool CanAddIngredient(IngredientInfo ingredient)
    {
        var tempIngredients =  new List<IngredientInfo>(_ingredients) { ingredient };

        return FoodManager.Instance.CheckPlateIngredients(tempIngredients);
    }
}
