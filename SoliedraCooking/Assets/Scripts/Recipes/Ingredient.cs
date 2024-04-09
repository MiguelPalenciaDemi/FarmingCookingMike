using System;
using System.Timers;
using UnityEngine;

public enum  IngredientState
{
   Raw, MediumRare ,Cooked, Overcooked, Chopped, Smashed
}
public class Ingredient : MonoBehaviour
{
   [SerializeField] private IngredientInfo ingredientInfo;
   [SerializeField] private Transform modelParent;
   private IngredientState _ingredientState;
   private float _timer;

   public void Start()
   {
      _ingredientState = IngredientState.Raw;
      UpdateModel();
   }

   // ReSharper disable Unity.PerformanceAnalysis
   public void Chop()
   {
      if (!ingredientInfo.IsChoppable || _ingredientState == IngredientState.Chopped) return;

      if (_timer < ingredientInfo.TimeChop)
      {
         _timer += Time.deltaTime;
         Debug.Log("Chopping: "+_timer);
         //Update UI
      }
      else
      {
         //Finish chopping
         _ingredientState = IngredientState.Chopped;
         _timer = 0;
         UpdateModel();
      }
      
   }

   // ReSharper disable Unity.PerformanceAnalysis
   public void Smash()
   {
      
      if(!ingredientInfo.IsSmashable || _ingredientState == IngredientState.Smashed) return;
      
      if (_timer < ingredientInfo.TimeSmash)
      {
         _timer += Time.deltaTime;
         Debug.Log("Smashing: "+_timer);
         //Update UI
      }
      else
      {
         //Finish chopping
         _ingredientState = IngredientState.Smashed;
         _timer = 0;
         UpdateModel();
      }
   }
   
   
   public void UpdateModel()
   {
      var model = ingredientInfo.GetModel(_ingredientState); //Get the new model from ingredient data
      if (!model) return; //None model found we avoid to delete current food model
      
      CleanModel();
      var newModel = Instantiate(model, modelParent);
      newModel.transform.localPosition = Vector3.zero;
      newModel.transform.localRotation = Quaternion.identity;
   }

   private void CleanModel()
   {
      foreach(Transform child in modelParent)
      {
         Destroy(child.gameObject);
      }
   }

   public FoodTag GetFoodTag()
   {
      return ingredientInfo.FoodTag;
   }

   public IngredientState GetState()
   {
      return _ingredientState;
   }

   public IngredientInfo GetIngredientInfo()
   {
      return ingredientInfo;
   }
}
