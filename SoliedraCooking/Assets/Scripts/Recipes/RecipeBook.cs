using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newRecipeBook",menuName = "RecipeBook")]
public class RecipeBook : ScriptableObject
{
    [SerializeField] private List<Recipe> recipes;

    public Recipe GetRandomRecipe()
    {
        var randomIndex = Random.Range(0, recipes.Count);
        return recipes[randomIndex];
    }
    
    
    
}