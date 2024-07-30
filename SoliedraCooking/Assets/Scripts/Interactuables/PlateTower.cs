using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class PlateTower : MonoBehaviour, ITakeDrop
{
    [SerializeField] private GameObject platePrefab;
    
    [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
    public void TakeDrop(PlayerInteract player)
    {
        
        
        // ReSharper disable once Unity.PerformanceCriticalCodeNullComparison
        if (player.ObjectPickedUp == null ) //Si no tenemos algo en la mano lo podemos coger
        {
            player.TakeObject(Instantiate(platePrefab));
            Debug.Log("Coger Objeto");
            
        }
        else if(player.ObjectPickedUp)
        {
            if(player.ObjectPickedUp.GetComponent<Plate>() || player.ObjectPickedUp.GetComponent<Pot>())//Comprobamos que tengamos un plato o una olla en la mano
            {
                Destroy(player.DropObject());
            }
        }
    }

    public bool CanTakeDrop()
    {
        return true;
    }
}
