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

        UpdateModel();
        return true;

        //Change Model depend on ingredients in plate
    }

    private void UpdateModel()
    {
        var model = FoodManager.Instance.GetFoodModel(_ingredients);
        if(model)
            SetModel(model);
    }

    private void SetModel(GameObject model)
    {
        //primero limpiamos el estado actual
        RemoveModel();
        
        //Instanciamos el nuevo
        var newModel = Instantiate(model,modelParent);
        
        newModel.transform.localPosition = Vector3.zero;
        newModel.transform.localRotation = Quaternion.identity;
        
    }
    public void Clean()
    {
        _ingredients.Clear();

        RemoveModel();
    }

    private void RemoveModel()
    {
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

    public bool Fill(List<IngredientInfo> ingredients)
    {
        if (_ingredients.Count > 0) return false;
        
        Debug.Log("LLenar plato");

        _ingredients = new List<IngredientInfo>(ingredients);
        UpdateModel();
        
        return true;
    }
}
