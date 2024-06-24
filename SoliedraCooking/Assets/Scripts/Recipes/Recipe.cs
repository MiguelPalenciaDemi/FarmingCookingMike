using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


[CreateAssetMenu(fileName = "newRecipe",menuName = "Recipe")]
public class Recipe : ScriptableObject
{
    [SerializeField] private List<IngredientInfo> ingredients;
    [SerializeField] private Sprite recipeImage;
    [SerializeField] private float timeToPrepare;
    [SerializeField, TextAreaAttribute] private string description;
    public List<IngredientInfo> Ingredients => ingredients;
    public Sprite RecipeImage => recipeImage;
    public float TimeToPrepare => timeToPrepare;

    public string Description => description.ToString();

    //Compromabos si la receta está realizada correctamente.
    public bool ComparePlate(List<IngredientInfo> plateIngredients)
    {
        if (plateIngredients.Count != ingredients.Count) return false;

        return CheckIngredients(plateIngredients);
    }

    //Nos va a indicar si estamos haciendo correctamente una receta (para ver si podemos añadir un ingrediente o no)
    public bool CheckProcessRecipe(List<IngredientInfo> plateIngredients)
    {
        if (plateIngredients.Count > ingredients.Count) return false;
        
        return CheckIngredients(plateIngredients);
    }

    private bool CheckIngredients(List<IngredientInfo> plateIngredients)
    {
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





