using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class Worktop : Workstation
{
   [Header("Audio Actions")] 
   [SerializeField] private EventReference chopEvent;
   [SerializeField] private EventReference smashEvent;
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
         player.ChopAnimation();
         AudioManager.Instance.PlaySoundAtPosition(chopEvent,transform);
         ingredient.Chop(this,speedCooking);
         widgetUI.UpdateUI(ingredient.GetChopProgress());
      }
      else if (info.CanDoAction(CookAction.Smash))
      {
         //ShowUI(IngredientState.Smashed,true);
         player.SmashAnimation();
         AudioManager.Instance.PlaySoundAtPosition(smashEvent,transform);
         ingredient.Smash(this,speedCooking);
         widgetUI.UpdateUI(ingredient.GetSmashProgress());
      }
         
   }
   // Sobreescribimos para añadir la funcionalidad de mezclar ingredientes
   public override void TakeDrop(PlayerInteract player)
   {
      
     
      base.TakeDrop(player);
   }

   
}
