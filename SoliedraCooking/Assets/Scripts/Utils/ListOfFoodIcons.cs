using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct FoodIconRecipeStruct
{
    [SerializeField] private FoodTag foodTag;
    public FoodTag Tag => foodTag;


    [SerializeField] private Sprite icon;
    public Sprite Icon => icon;
}
[CreateAssetMenu(fileName = "newListOfFoodIcons",menuName = "ListOfFoodIcons")]
public class ListOfFoodIcons : ScriptableObject
{
    [SerializeField] private List<FoodIconRecipeStruct> icons;

    public List<FoodIconRecipeStruct> Icons => icons;
}
