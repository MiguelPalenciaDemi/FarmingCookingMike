using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCrate : MonoBehaviour, ITakeDrop
{
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private int amount;
    private FoodTag.FoodItem _foodItem;

    private void Awake()
    {
        _foodItem = foodPrefab.GetComponent<FoodTag>().foodTag;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void TakeDrop(PlayerInteract player)
    {
        
        if (player.ObjectPickedUp == null && amount>0) //Si no tenemos algo en la mano lo podemos coger
        {
            player.TakeObject(Instantiate(foodPrefab));
            Debug.Log("Coger Objeto");
            amount--;
        }
        else if(player.ObjectPickedUp)
        {
            var objectPickedTag = player.ObjectPickedUp.GetComponent<FoodTag>().foodTag;
            if(objectPickedTag == _foodItem)//Check if we've got the same foodType in the hand
            {
                Debug.Log("Dejar Objeto");
                amount++;
                Destroy(player.DropObject());
            }
        }
    }


    
}
