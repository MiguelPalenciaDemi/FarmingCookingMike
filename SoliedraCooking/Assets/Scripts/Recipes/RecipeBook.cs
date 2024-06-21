using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "newRecipeBook",menuName = "RecipeBook")]
public class RecipeBook : ScriptableObject
{
    [SerializeField] private List<Recipe> recipes;

    public List<Recipe> Recipes => recipes;

    public Recipe GetRandomRecipe()
    {
        var randomIndex = Random.Range(0, recipes.Count);
        return recipes[randomIndex];
    }

    public bool SearchMatchRecipe(List<IngredientInfo> ingredients)
    {
        return recipes.Any(recipe => recipe.ComparePlate(ingredients));
    }

    //Comprueba que los ingredientes coinciden con alguna receta
    public bool ValidIngredients(List<IngredientInfo> ingredients)
    {
        return recipes.Any(recipe => recipe.CheckProcessRecipe(ingredients));
    }
    
}