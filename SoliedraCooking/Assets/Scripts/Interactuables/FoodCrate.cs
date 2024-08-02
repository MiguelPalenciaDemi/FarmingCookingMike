using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FoodCrate : MonoBehaviour, ITakeDrop
{
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private IngredientInfo ingredientInfo;
    [SerializeField] private int amount;
    [SerializeField] private TextMeshProUGUI counterUI;
    [SerializeField] private Mesh crateEmpty;
    private FoodTag _foodItem;
    private MeshFilter currentMesh;
    private Mesh originalMesh;
    private int Amount
    {
        get => amount;
        set
        {
            amount = value;
            CheckEmpty();
            UpdateUI();
        }
    }

    private void CheckEmpty()
    {
        currentMesh.mesh = amount == 0 ? crateEmpty : originalMesh;
    }

    private void Awake()
    {
        _foodItem = ingredientInfo.FoodTag;
        currentMesh = GetComponent<MeshFilter>();
        originalMesh = currentMesh.mesh;
        
        CheckEmpty();
        UpdateUI();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void TakeDrop(PlayerInteract player)
    {
        
        if (player.ObjectPickedUp == null && amount>0) //Si no tenemos algo en la mano lo podemos coger
        {
            var food = Instantiate(foodPrefab);
            food.TryGetComponent<Ingredient>(out var ingredient);
            if (!ingredient)
            {
                Destroy(food);
                return; //Si no contiene el script Ingredient no hacemos nada
            }
            ingredient.SetIngredientInfo(ingredientInfo);//Le indicamos que ingrediente es.
            player.TakeObject(food);
            Debug.Log("Coger Objeto");
            Amount--;
        }
        else if(player.ObjectPickedUp && player.ObjectPickedUp.TryGetComponent(out Ingredient ingredient))
        {
            //var ingredient = player.ObjectPickedUp.GetComponent<Ingredient>();
            
            if(ingredient.GetIngredientInfo() == ingredientInfo)//Check if we've got the same foodType in the hand and it's raw
            {
                Debug.Log("Dejar Objeto");
                Amount++;
                Destroy(player.DropObject());
            }
        }
    }

    public bool CanTakeDrop()
    {
        return amount > 0;
    }

    private void UpdateUI()
    {
        counterUI.text = amount.ToString();
    }

    public void SetCrateAmount(int newValue)
    {
        Amount = newValue;

    }
}
