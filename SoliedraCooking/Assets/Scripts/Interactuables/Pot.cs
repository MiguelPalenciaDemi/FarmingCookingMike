using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    [SerializeField] private float cookTime = 10f;
    
    private List<IngredientInfo> _ingredients;
    private bool _hasWater = false;
    private bool _isCooking = false;
    
    //Parte similar al ingrediente
    private IngredientState _ingredientState;
    private float _cookingTimer = 0; //Necesitamos un timer diferente, para que no se resetee, por eso cada vez que se inicie la corrutina tampoco lo ponemos a 0
    private Coroutine _cookingCoroutine;
    private Workstation _workstation;//Donde está siendo tratado

    public bool AddIngredient(IngredientInfo ingredient)
    {
        if (!_hasWater &&!CanAddIngredient(ingredient) && !_isCooking) return false;
        
        _ingredients.Add(ingredient);
        return true;

    }

    private bool CanAddIngredient(IngredientInfo ingredient)
    {
        var tempIngredients = _ingredients;
        tempIngredients.Add(ingredient);
        
        return FoodManager.Instance.CheckPotIngredients(tempIngredients);
    }

    public void AddWater()
    {
        _hasWater = true;
    }

    public bool CanCook()
    {
        return _hasWater && FoodManager.Instance.CheckPotIngredients(_ingredients);
    }

    public void Cook(Workstation workstation, float modifier)
    {
        _workstation = workstation;
        _cookingCoroutine = StartCoroutine(Cooking(modifier));

    }
    
    public void StopCook()
    {
        if (_cookingCoroutine == null) return;
        StopCoroutine(_cookingCoroutine);
      
        // //Si está cocinado o "quemado" cambiaremos el modelo
        // if(_ingredientState == IngredientState.Cooked) 
        //     UpdateModel(ingredientInfo.CompleteAction(CookAction.Cook));
        // else if(_ingredientState == IngredientState.Overcooked)
        //     UpdateModel(ingredientInfo.CompleteAction(CookAction.Cook),true);
        //
        // _cookingCoroutine = null;
        // _workstation = null;

    }
    private IEnumerator Cooking(float modifier)
    {
        while (_ingredientState != IngredientState.Overcooked)
        {
            _workstation.Warning(false);

            while(_ingredientState != IngredientState.Cooked)//Proceso para que se cocine la comida
            {
                Debug.Log("Cooking: " +_cookingTimer);
                _cookingTimer += Time.deltaTime * modifier;
            
                //Actualizamos la UI
                _workstation.UpdateUI(GetCookedProgress());

                if (_cookingTimer > cookTime)
                {
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
                Debug.Log("OverCooking: " +_cookingTimer);
                _cookingTimer += Time.deltaTime;
            
                //Actualizamos la UI
                _workstation.UpdateUI(GetOvercookedProgress());

                if (_cookingTimer > cookTime * 0.5f)
                {
                    _ingredientState = IngredientState.Overcooked;
                    //UpdateModel(); AQUI DEBERIAMOS LLAMAR AL NUEVO INGREDIENTE- SE TRASLADA AL ABRIR EL HORNO
                    _workstation.ForceStopInteract();
                    //Do VFX things
                }

                yield return null;
            }

        }

        yield return null;
    }

    public float GetCookedProgress()
    {
        return _cookingTimer / cookTime;
    }
   
    public float GetOvercookedProgress()
    {
        return _cookingTimer / (cookTime * 0.5f);
    }
}
