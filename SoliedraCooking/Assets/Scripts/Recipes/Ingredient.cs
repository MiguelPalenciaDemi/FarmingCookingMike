using System;
using System.Collections;
using System.Timers;
using UnityEngine;


public class Ingredient : MonoBehaviour
{
   [SerializeField] private IngredientInfo ingredientInfo;
   [SerializeField] private Transform modelParent;
   private IngredientState _ingredientState;
   private float _chopTimer;
   private float _cookingTimer = 0; //Necesitamos un timer diferente, para que no se resetee, por eso cada vez que se inicie la corrutina tampoco lo ponemos a 0
   private float _overcookTime = 0;
   private Coroutine _cookingCoroutine;
   private Workstation _workstation;//Donde está siendo tratado

   private ProgressWidget _widget;
   public void Start()
   {
      _ingredientState = IngredientState.Raw;
      if(ingredientInfo)
         UpdateModel(ingredientInfo);
   }

   // ReSharper disable Unity.PerformanceAnalysis
   public void Chop(Workstation station, float modifier = 1)
   {
      if (!ingredientInfo.CanDoAction(CookAction.Chop)) return;

      if (_chopTimer < ingredientInfo.GetTime(CookAction.Chop))
      {
         _chopTimer += Time.deltaTime * modifier;
         Debug.Log("Chopping: "+_chopTimer);
         //Update UI
         station.ShowProgressUI(true, CookAction.Chop);
         station.ShowInteractUI(false);
         station.UpdateUI(GetChopProgress());
      }
      else
      {
         //Finish chopping
         _ingredientState = IngredientState.Chopped;
         _chopTimer = 0;
         
         UpdateModel(ingredientInfo.CompleteAction(CookAction.Chop));
         station.ShowProgressUI(false);
         station.ShowInteractUI(true,this.gameObject);
      }
      
   }

   // ReSharper disable Unity.PerformanceAnalysis
   public void Smash(Workstation station,float modifier = 1)
   {
      
      if(!ingredientInfo.CanDoAction(CookAction.Smash)) return;
      
      if (_chopTimer < ingredientInfo.GetTime(CookAction.Smash))
      {
         _chopTimer += Time.deltaTime * modifier;
         Debug.Log("Smashing: "+_chopTimer);
         
         station.ShowProgressUI(true, CookAction.Smash);
         station.ShowInteractUI(false);
         station.UpdateUI(GetSmashProgress());
      }
      else
      {
         //Finish Smashing
         _ingredientState = IngredientState.Smashed;
         
         UpdateModel(ingredientInfo.CompleteAction(CookAction.Smash));
         station.ShowProgressUI(false);
         station.ShowInteractUI(true, this.gameObject);
         
         _chopTimer = 0;
      }
   }

   public void Cook(Workstation workstation,float modifier = 1)
   {
      _workstation = workstation;
      _cookingCoroutine = StartCoroutine(Cooking(modifier));
      
   }

   public void StopCook()
   {
      if (_cookingCoroutine == null) return;
      
      StopCoroutine(_cookingCoroutine);
      _cookingCoroutine = null;
      _workstation = null;
      
      // //Si está cocinado o "quemado" cambiaremos el modelo
      // if(_ingredientState == IngredientState.Cooked) 
      //    UpdateModel(ingredientInfo.CompleteAction(CookAction.Cook));
      // else if(_ingredientState == IngredientState.Overcooked)
      //    UpdateModel(ingredientInfo.CompleteAction(CookAction.Cook),true);
      //

   }

   // ReSharper disable Unity.PerformanceAnalysis
   private IEnumerator Cooking(float modifier)
   {
      var cookTime = ingredientInfo.GetTime(CookAction.Cook);
      while (_ingredientState != IngredientState.Overcooked)
      {
         _workstation.Warning(false);

         while(_ingredientState != IngredientState.Cooked)//Proceso para que se cocine la comida
         {
            
            _cookingTimer += Time.deltaTime * modifier;
            
            //Actualizamos la UI
            _workstation.UpdateUI(GetCookedProgress());

            if (_cookingTimer > cookTime)
            {
               UpdateModel(ingredientInfo.CompleteAction(CookAction.Cook));
               _ingredientState = IngredientState.Cooked;
               //UpdateModel(); AQUI DEBERIAMOS LLAMAR AL NUEVO INGREDIENTE- SE TRASLADA AL ABRIR EL HORNO
               _cookingTimer = 0;
               //Do VFX things
            }

            yield return null;
         }
         
         _workstation.Warning(true);
         while(_ingredientState == IngredientState.Cooked)//Proceso para que se queme la comida
         {
            
            _cookingTimer += Time.deltaTime;
            
            //Actualizamos la UI
            _workstation.UpdateUI(GetOvercookedProgress());

            if (_cookingTimer > cookTime * 0.5f)
            {
               UpdateModel(ingredientInfo,true);
               _ingredientState = IngredientState.Overcooked;
               
               _workstation.ForceStopInteract();
               //Do VFX things
            }

            yield return null;
         }

      }

      yield return null;
   }
   
   public void UpdateModel(IngredientInfo ingredient, bool isSpoiled = false)
   {
      
      var model = isSpoiled? ingredient.GetSpoiledModel() : ingredient.GetModel(); //Get the new model from ingredient data, if it's spoiled 
      if (!model) return; //None model found we avoid to delete current food model
      
      CleanModel();
      var newModel = Instantiate(model, modelParent);
      newModel.transform.localPosition = Vector3.zero;
      newModel.transform.localRotation = Quaternion.identity;

      _overcookTime = ingredientInfo.GetTime(CookAction.Cook) * 0.5f;//Antes de cambiar el ingrediente calculamos su tiempo para quemarse, sino perderemos esa info
      
      ingredientInfo = ingredient;
      _ingredientState = IngredientState.Raw;
   }

   private void CleanModel()
   {
      foreach(Transform child in modelParent)
      {
         Destroy(child.gameObject);
      }
   }

   public bool CanDoAction(CookAction action)
   {
      return ingredientInfo.CanDoAction(action);
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
 
   public float GetChopProgress()
   {
      return _chopTimer / ingredientInfo.GetTime(CookAction.Chop);
   }
   
   public float GetSmashProgress()
   {
      return _chopTimer / ingredientInfo.GetTime(CookAction.Smash);
   }
   
   public float GetCookedProgress()
   {
      return _cookingTimer / ingredientInfo.GetTime(CookAction.Cook);
   }
   
   public float GetOvercookedProgress()
   {
      return _cookingTimer /_overcookTime;
   }

   public void CleanWorkstation()
   {
      _workstation = null;
   }

   public void SetIngredientInfo(IngredientInfo info)
   {
      ingredientInfo = info;
      UpdateModel(ingredientInfo);
   }
   
}
