using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeTitleUI : MonoBehaviour
{
    [SerializeField] private Image imageRecipe;
    [SerializeField] private TextMeshProUGUI titleRecipe;

    public void SetUp(Sprite image, string text)
    {
        imageRecipe.sprite = image;
        titleRecipe.text = text;
    }
}
