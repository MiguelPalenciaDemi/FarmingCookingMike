using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workstation : MonoBehaviour, IInteractable,ITakeDrop
{
    [SerializeField] protected Transform objectPosition;
    [SerializeField] protected float speedCooking = 1;
    protected GameObject _objectInWorktop;


    public virtual void Interact(PlayerInteract player)
    {
      
    }
    
    public virtual void ForceStopInteract()
    {
      
    }

    public virtual void TakeDrop(PlayerInteract player)
    {
        // ReSharper disable once Unity.PerformanceCriticalCodeNullComparison
        if (player.ObjectPickedUp == null && _objectInWorktop) //Si no tenemos algo en la mano lo podemos coger
        {
            player.TakeObject(_objectInWorktop);
            _objectInWorktop = null;
        }
        else if(player.ObjectPickedUp && !_objectInWorktop)
        {
            _objectInWorktop =  player.DropObject();
            _objectInWorktop.transform.parent = objectPosition;
            _objectInWorktop.transform.position = objectPosition.position;
        }
        // else if(player.ObjectPickedUp && _objectInWorktop) //Tenemos algo en la mano y en la encimera
        // {
        //     if (player.ObjectPickedUp.TryGetComponent(out Ingredient ingredient) &&
        //         _objectInWorktop.TryGetComponent(out Plate plate)) //Tenemos en la mano un ingrediente y en la encimera un plato
        //     {
        //         plate.AddIngredient(ingredient.GetIngredientStruct());
        //         Destroy(player.DropObject());
        //     }
        // }
    }
}
