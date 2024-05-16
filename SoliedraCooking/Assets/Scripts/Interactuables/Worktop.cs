using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worktop : Workstation
{
   
   // ReSharper disable Unity.PerformanceAnalysis
   public override void Interact(PlayerInteract player)
   {
      if(!_objectInWorktop) return; //If there isn't any obj to interact we exit
      
      _objectInWorktop.TryGetComponent(out Ingredient ingredient);
      
      //Is an ingredient?  
      if (!ingredient) return;
      
      var state = ingredient.GetState();
      var info = ingredient.GetIngredientInfo();


      if (info.CanDoAction(CookAction.Chop))
      {
         //ShowUI(IngredientState.Chopped,true);

         ingredient.Chop(this,speedCooking);
         widgetUI.UpdateUI(ingredient.GetChopProgress());
      }
      else if (info.CanDoAction(CookAction.Smash))
      {
         //ShowUI(IngredientState.Smashed,true);
         ingredient.Smash(this,speedCooking);
         widgetUI.UpdateUI(ingredient.GetSmashProgress());
      }
         
   }

   public override void TakeDrop(PlayerInteract player)
   {
      if (_objectInWorktop && player.ObjectPickedUp)
      {
         _objectInWorktop.TryGetComponent(out Plate plate);
         
         //Is a Plate and we have an ingredient in our hand?
         if (plate)
         {
            player.ObjectPickedUp.TryGetComponent(out Ingredient playerIngredient);
            if (!playerIngredient) return;
            plate.AddIngredient(playerIngredient.GetIngredientInfo());
            Destroy(player.DropObject());

            return;
         }
         
         //Ingrediente en la encimera - Plato en la mano -> juntamos ingredientes y ponemos plato.
         _objectInWorktop.TryGetComponent(out Ingredient ingredient);
         if (ingredient)
         {
            player.ObjectPickedUp.TryGetComponent(out plate);
            if (!plate) return;
            plate.AddIngredient(ingredient.GetIngredientInfo());
            Destroy(ingredient.gameObject);
            DropObject(player);
            ShowInteractUI(false);

            return;
         }
      }
     
      base.TakeDrop(player);
   }

   
}
