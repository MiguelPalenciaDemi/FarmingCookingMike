using System;
using System.Collections;
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
   private float _chopTimer;
   private float _cookingTimer = 0; //Necesitamos un timer diferente, para que no se resetee, por eso cada vez que se inicie la corrutina tampoco lo ponemos a 0
   private Coroutine _cookingCoroutine;
   private Workstation _workstation;//Donde está siendo tratado
   public void Start()
   {
      _ingredientState = IngredientState.Raw;
      UpdateModel();
   }

   // ReSharper disable Unity.PerformanceAnalysis
   public void Chop(float modifier = 1)
   {
      if (!ingredientInfo.IsChoppable || _ingredientState == IngredientState.Chopped) return;

      if (_chopTimer < ingredientInfo.TimeChop)
      {
         _chopTimer += Time.deltaTime * modifier;
         Debug.Log("Chopping: "+_chopTimer);
         //Update UI
      }
      else
      {
         //Finish chopping
         _ingredientState = IngredientState.Chopped;
         _chopTimer = 0;
         UpdateModel();
      }
      
   }

   // ReSharper disable Unity.PerformanceAnalysis
   public void Smash(float modifier = 1)
   {
      
      if(!ingredientInfo.IsSmashable || _ingredientState == IngredientState.Smashed) return;
      
      if (_chopTimer < ingredientInfo.TimeSmash)
      {
         _chopTimer += Time.deltaTime * modifier;
         Debug.Log("Smashing: "+_chopTimer);
         //Update UI
      }
      else
      {
         //Finish chopping
         _ingredientState = IngredientState.Smashed;
         _chopTimer = 0;
         UpdateModel();
      }
   }

   public void Cook(Workstation workstation,float modifier = 1)
   {
      Debug.Log("Estoy en Cook");
      _workstation = workstation;
      _cookingCoroutine = StartCoroutine(Cooking(modifier));
      
   }

   public void StopCook()
   {
      if (_cookingCoroutine == null) return;
      StopCoroutine(_cookingCoroutine);
      
      _cookingCoroutine = null;
      _workstation = null;

   }

   // ReSharper disable Unity.PerformanceAnalysis
   private IEnumerator Cooking(float modifier)
   {
      
      while (_ingredientState != IngredientState.Overcooked)
      {
         var maxTimer = ingredientInfo.TimeCooking + ingredientInfo.TimeOverCook;
         if (_cookingTimer < maxTimer)
         {
            Debug.Log("Cooking: " +_chopTimer);

            _cookingTimer += Time.deltaTime;

            if (_cookingTimer > ingredientInfo.TimeCooking && _ingredientState != IngredientState.Cooked)
            {
               _ingredientState = IngredientState.Cooked;
               UpdateModel();
               //Do UI things
            }

            yield return null;
         }
         else
         {
            _ingredientState = IngredientState.Overcooked;
            UpdateModel();
            _workstation.ForceStopInteract();
         }

      }

      yield return null;
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
