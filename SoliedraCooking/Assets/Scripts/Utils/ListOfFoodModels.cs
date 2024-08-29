using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "IngredientModels", fileName = "ListOfModels")]
public class ListOfFoodModels : ScriptableObject
{
    [SerializeField] private List<FoodModel> listModel;

    public GameObject GetModel(List<IngredientInfo> ingredients)
    {
        foreach (var item in listModel)
        {
            if (item.MatchIngredients(ingredients))
                return item.Model;
        }

        return null;
    }
}
