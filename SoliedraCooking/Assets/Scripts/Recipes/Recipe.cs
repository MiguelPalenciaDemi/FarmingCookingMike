using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newRecipe",menuName = "Recipe")]
public class Recipe : ScriptableObject
{
    [SerializeField] private List<IngredientInfo> ingredients;
    [SerializeField] private Sprite recipeImage;
    public List<IngredientInfo> Ingredients => ingredients;

    public Sprite RecipeImage => recipeImage;


    public bool ComparePlate(List<IngredientInfo> plateIngredients)
    {
        if (plateIngredients.Count != ingredients.Count) return false;
        
        var tempList = new List<IngredientInfo>(ingredients);

        foreach (var otherIngredient in plateIngredients) //Comprobamos todos los componentes del plato
        {
            var index = tempList.FindIndex(x =>
                x == otherIngredient); //Buscamos que nuestro ingrediente se encuentre en la receta

            if (index == -1)
                return false;
            
            tempList.RemoveAt(index);
        }
        
        return true;
    }
    
    
}





