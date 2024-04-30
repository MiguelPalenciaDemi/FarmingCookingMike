using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI textRecipe;
    [SerializeField] private GameObject completeUI;

    public void Complete()
    {
        completeUI.SetActive(true);
    }

    public void SetUp(Sprite spriteRecipe,string nameRecipe)
    {
        image.sprite = spriteRecipe;
        textRecipe.text = nameRecipe;
    }
}
