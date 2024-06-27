using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class RecipeInfoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameRecipeText;
    [SerializeField] private TextMeshProUGUI infoRecipeText;
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject imagePrefab;

    public void Setup(Recipe recipeSelected)
    {
        nameRecipeText.text = recipeSelected.name;
        infoRecipeText.text = recipeSelected.Description;
        
        ClearList();
        foreach (var ingredient in recipeSelected.Ingredients)
        {
            var item = Instantiate(imagePrefab, contentParent).GetComponent<Image>();
            item.sprite = FoodManager.Instance.GetFoodIcon(ingredient.FoodTag);
        }
    }
    
    public void ClearList()
    {
        //Remove model
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }
    }

}
