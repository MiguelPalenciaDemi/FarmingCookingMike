using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    [SerializeField] private float timeCooking = 10f;
    
    private List<IngredientInfo> _ingredients;
    private bool _hasWater = false;


    public bool AddIngredient(IngredientInfo ingredient)
    {
        if (!CanAddIngredient(ingredient)) return false;
        
        _ingredients.Add(ingredient);
        return true;

    }

    private bool CanAddIngredient(IngredientInfo ingredient)
    {
        var tempIngredients = _ingredients;
        tempIngredients.Add(ingredient);
        
        return FoodManager.Instance.CheckPotIngredients(tempIngredients);
    }

}
