using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worktop : Workstation
{
   
   // ReSharper disable Unity.PerformanceAnalysis
   public override void Interact(PlayerInteract player)
   {
      if(!_objectInWorktop) return; //If there isn't any obj to interact we exit
      

      _objectInWorktop.TryGetComponent(out Plate plate);
      _objectInWorktop.TryGetComponent(out Ingredient ingredient);
      
      //Is a Plate and we have an ingredient in our hand?
      if (plate && player.ObjectPickedUp)
      {
         player.ObjectPickedUp.TryGetComponent(out Ingredient playerIngredient);
         if (playerIngredient)
         {
            plate.AddIngredient(playerIngredient.GetIngredientStruct());
            Destroy(player.DropObject());
         }

      }
      else
      {
         //Is an ingredient?  
         if (!ingredient) return;
         
         
         var state = ingredient.GetState();
         var info = ingredient.GetIngredientInfo();
         
         
         if(info.IsChoppable && state != IngredientState.Chopped && state != IngredientState.Smashed)
            ingredient.Chop(speedCooking);
         
         else if(info.IsSmashable && state != IngredientState.Smashed)
            ingredient.Smash(speedCooking);
         
      }
      
      
   }
   
}
