using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
[CreateAssetMenu(fileName = "newRecipe",menuName = "Recipe")]
public class Recipe : ScriptableObject
{
    [SerializeField] private List<IngredientRecipeStruct> ingredients;
    [SerializeField] private Sprite recipeImage;
    public List<IngredientRecipeStruct> Ingredients => ingredients;

    public Sprite RecipeImage => recipeImage;


    public bool ComparePlate(List<IngredientRecipeStruct> plateIngredients)
    {
        if (plateIngredients.Count != ingredients.Count) return false;
        
        var tempList = new List<IngredientRecipeStruct>(ingredients);

        foreach (var otherIngredient in plateIngredients) //Comprobamos todos los componentes del plato
        {
            var index = tempList.FindIndex(x =>
                x.Compare(otherIngredient)); //Buscamos que nuestro ingrediente se encuentre en la receta

            if (index == -1)
                return false;
            
            tempList.RemoveAt(index);
        }
        
        return true;
    }
    
    
}





