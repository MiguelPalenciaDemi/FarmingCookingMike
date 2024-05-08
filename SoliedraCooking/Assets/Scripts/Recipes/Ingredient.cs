using System;
using System.Collections;
using System.Timers;
using UnityEngine;

public enum  IngredientState
{
   Raw, MediumRare ,Cooked, Overcooked, Chopped, Smashed, None
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

   private ProgressWidget _widget;
   public void Start()
   {
      _ingredientState = IngredientState.Raw;
      UpdateModel();
   }

   // ReSharper disable Unity.PerformanceAnalysis
   public void Chop(Workstation station, float modifier = 1)
   {
      if (!ingredientInfo.IsChoppable || _ingredientState == IngredientState.Chopped) return;

      if (_chopTimer < ingredientInfo.TimeChop)
      {
         _chopTimer += Time.deltaTime * modifier;
         Debug.Log("Chopping: "+_chopTimer);
         //Update UI
         station.ShowProgressUI(true, IngredientState.Chopped);
         station.ShowInteractUI(false);
         station.UpdateUI(GetChopProgress());
      }
      else
      {
         //Finish chopping
         _ingredientState = IngredientState.Chopped;
         _chopTimer = 0;
         station.ShowProgressUI(false);
         station.ShowInteractUI(true,this.gameObject);
         UpdateModel();
      }
      
   }

   // ReSharper disable Unity.PerformanceAnalysis
   public void Smash(Workstation station,float modifier = 1)
   {
      
      if(!ingredientInfo.IsSmashable || _ingredientState == IngredientState.Smashed) return;
      
      if (_chopTimer < ingredientInfo.TimeSmash)
      {
         _chopTimer += Time.deltaTime * modifier;
         Debug.Log("Smashing: "+_chopTimer);
         //Update UI
         //station.ShowUI(true, IngredientState.Smashed);
         station.ShowProgressUI(true, IngredientState.Smashed);
         station.ShowInteractUI(false);
         station.UpdateUI(GetSmashProgress());
      }
      else
      {
         //Finish Smashing
         _ingredientState = IngredientState.Smashed;
         station.ShowProgressUI(false);
         station.ShowInteractUI(true, this.gameObject);
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
         _workstation.Warning(false);

         while(_ingredientState != IngredientState.Cooked)//Proceso para que se cocine la comida
         {
            Debug.Log("Cooking: " +_cookingTimer);
            _cookingTimer += Time.deltaTime;
            
            //Actualizamos la UI
            _workstation.UpdateUI(GetCookedProgress());

            if (_cookingTimer > ingredientInfo.TimeCooking)
            {
               _ingredientState = IngredientState.Cooked;
               UpdateModel();
               _cookingTimer = 0;
               //Do VFX things
            }

            yield return null;
         }
         
         _workstation.Warning(true);
         while(_ingredientState == IngredientState.Cooked)//Proceso para que se queme la comida
         {
            Debug.Log("OverCooking: " +_cookingTimer);
            _cookingTimer += Time.deltaTime;
            
            //Actualizamos la UI
            _workstation.UpdateUI(GetOvercookedProgress());

            if (_cookingTimer > ingredientInfo.TimeOverCook)
            {
               _ingredientState = IngredientState.Overcooked;
               UpdateModel();
               _workstation.ForceStopInteract();
               //Do VFX things
            }

            yield return null;
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

   public IngredientRecipeStruct GetIngredientStruct()
   {
      return new IngredientRecipeStruct(GetFoodTag(), GetState());
   }

   public float GetChopProgress()
   {
      return _chopTimer / ingredientInfo.TimeChop;
   }
   
   public float GetSmashProgress()
   {
      return _chopTimer / ingredientInfo.TimeSmash;
   }
   
   public float GetCookedProgress()
   {
      return _cookingTimer / ingredientInfo.TimeCooking;
   }
   
   public float GetOvercookedProgress()
   {
      return _cookingTimer /ingredientInfo.TimeOverCook;
   }

   public void CleanWorkstation()
   {
      _workstation = null;
   }
   
}
