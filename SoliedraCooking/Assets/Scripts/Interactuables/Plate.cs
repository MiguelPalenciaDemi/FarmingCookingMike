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

    public void AddIngredient(IngredientInfo newIngredient)
    {
        //if(_ingredients.Exists(x => x.Compare(newIngredient))) //Comprobamos que ya tenemos este ingrediente
        _ingredients.Add(newIngredient);
        
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
}
