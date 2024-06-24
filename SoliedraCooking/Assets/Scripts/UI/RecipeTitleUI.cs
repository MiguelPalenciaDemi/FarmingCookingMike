using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeTitleUI : MonoBehaviour
{
    [SerializeField] private Image imageRecipe;
    [SerializeField] private TextMeshProUGUI titleRecipe;
    [SerializeField] private GameObject borderSelected;
    private Recipe _recipe;

    public Recipe Recipe => _recipe;

    public void SetUp(Recipe recipe)
    {
        _recipe = recipe;
        imageRecipe.sprite = _recipe.RecipeImage;
        titleRecipe.text = _recipe.name;
    }

    public void Select()
    {
        if (borderSelected.activeInHierarchy) return;
        
        ShowBorder(true);
        RecipebookUI.Instance.ShowSelectRecipeItem(this);
    }

    public void ShowBorder(bool value)
    {
        borderSelected.SetActive(value);
    }
}
