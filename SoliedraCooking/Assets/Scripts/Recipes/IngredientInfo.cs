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

[System.Serializable]
public struct CookActionStruct
{
    private CookActions _action;
    private IngredientInfo _ingredientResult;
    private float _time;

    public CookActions Action => _action;
    public IngredientInfo Result => _ingredientResult;
    public float Time => _time;

}
[CreateAssetMenu(fileName = "New_Ingredient", menuName = "Ingredient")]
public class IngredientInfo : ScriptableObject
{
    [SerializeField] private FoodTag _foodTag;

    [SerializeField] private List<CookActionStruct> actions;
    
    
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

    
    //Nos devuelve el nuevo IngredientInfo en el que se va a convertir
    public IngredientInfo CompleteAction(CookActions action)
    {
        var index = actions.FindLastIndex(x => x.Action == action);
        return index != -1 ? actions[index].Result : null; //Si lo encuentra mandamos el ingrediente en el que se convertirá
    }

    public float GetTime(CookActions action)
    {
        var index = actions.FindLastIndex(x => x.Action == action);
        return index != -1 ? actions[index].Time : 0; //Si lo encuentra mandamos el tiempo que conlleva
    }

    public bool CanDoAction(CookActions action)
    {
        var index = actions.FindLastIndex(x => x.Action == action);
        return index != -1; //Se puede realizar dicha acción
        
    }
}
