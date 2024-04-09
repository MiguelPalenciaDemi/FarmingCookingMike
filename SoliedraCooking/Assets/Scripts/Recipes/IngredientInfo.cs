using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct IngredientModels
{
    public GameObject rawModel;
    public GameObject mediumRare;
    public GameObject cooked;
    public GameObject overcooked;
    public GameObject chopped;
    public GameObject smashed;
        
}


[CreateAssetMenu(fileName = "New_Ingredient", menuName = "Ingredient")]
public class IngredientInfo : ScriptableObject
{
    [SerializeField] private FoodTag _foodTag;
    
    
    
    
    [SerializeField] private IngredientModels ingredientModels;
    
    [Header("Cooking Properties")]
    [SerializeField] private float timeCooking;
    [SerializeField] private float timeOverCook;
    [SerializeField] private float timeChop;
    [SerializeField] private float timeSmash;
    [SerializeField] private bool isChoppable;
    [SerializeField] private bool isSmashable;


    public IngredientModels Models => ingredientModels;

    public GameObject GetModel(IngredientState state)
    {
        return state switch
        {
            IngredientState.Raw => ingredientModels.rawModel,
            IngredientState.MediumRare => ingredientModels.mediumRare,
            IngredientState.Cooked => ingredientModels.cooked,
            IngredientState.Overcooked => ingredientModels.overcooked,
            IngredientState.Chopped => ingredientModels.chopped,
            IngredientState.Smashed => ingredientModels.smashed,
            _ => ingredientModels.rawModel
        };
    }
    public bool IsChoppable => isChoppable;
    public bool IsSmashable => isSmashable;
    public float TimeCooking => timeCooking;
    public float TimeOverCook => timeOverCook;
    public float TimeChop => timeChop;
    public float TimeSmash => timeSmash;

    public FoodTag FoodTag => _foodTag;
}
