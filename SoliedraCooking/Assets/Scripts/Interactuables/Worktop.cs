using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worktop : Workstation
{
   
   // ReSharper disable Unity.PerformanceAnalysis
   public override void Interact(PlayerInteract player)
   {
      if(!_objectInWorktop) return; //If there isn't any obj to interact we exit
      //First get the ingredient   
      _objectInWorktop.TryGetComponent(out Ingredient ingredient);
      if (!ingredient) return;
      
      
      var state = ingredient.GetState();
      var info = ingredient.GetIngredientInfo();
      
      
      if(info.IsChoppable && state != IngredientState.Chopped && state != IngredientState.Smashed)
         ingredient.Chop();
      
      else if(info.IsSmashable && state != IngredientState.Smashed)
         ingredient.Smash();
      
   }
   
}
