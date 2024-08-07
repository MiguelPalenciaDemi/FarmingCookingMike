using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Pot : MonoBehaviour
{
    [SerializeField] private float cookTime = 10f;
    [SerializeField] private Transform modelParent;
    [SerializeField] private int maxRations = 4;
    [SerializeField] private IngredientInfo waterIngredient;
    [SerializeField] private GameObject waterModel;
    [SerializeField] private Material normalWaterMaterial;
    [SerializeField] private Material soupWaterMaterial;
        
    private List<IngredientInfo> _ingredients;
    private bool _hasWater = false;
    private bool _isCooking = false;

    private float _rations;
    //Parte similar al ingrediente
    private IngredientState _ingredientState;
    private float _cookingTimer = 0; //Necesitamos un timer diferente, para que no se resetee, por eso cada vez que se inicie la corrutina tampoco lo ponemos a 0
    private Coroutine _cookingCoroutine;
    private Workstation _workstation;//Donde está siendo tratado

    private MeshRenderer _waterMeshRenderer;

    public List<IngredientInfo> Ingredients => _ingredients;

    private void Awake()
    {
        _waterMeshRenderer = waterModel.GetComponent<MeshRenderer>();
        _waterMeshRenderer.material = normalWaterMaterial;
        _ingredients = new List<IngredientInfo>();
        _rations = maxRations;
        

    }

    public bool AddIngredient(IngredientInfo ingredient)
    {
        //Si no tenemos agua o no es un ingrediente para POT pasamos
        if (!_hasWater || !CanAddIngredient(ingredient)) return false;
        //Si esta cocinando tampoco se puede añadir
        if (_isCooking) return false;
        
        _ingredients.Add(ingredient);
        UpdateModel();
        
        // if(_workstation)
        //     _workstation.ShowInteractUI(true,gameObject);

        return true;

    }

    private void UpdateModel()
    {
        var model = FoodManager.Instance.GetFoodModelPot(_ingredients);

        if (model)
            SetModel(model);
    }

    private void SetModel(GameObject model)
    {
        //primero limpiamos el estado actual
        RemoveModel();
        
        //Instanciamos el nuevo
        var newModel = Instantiate(model,modelParent);
        
        newModel.transform.localPosition = Vector3.zero;
        newModel.transform.localRotation = Quaternion.identity;
        
    }
    private void RemoveModel()
    {
        //Remove model
        foreach (Transform model in modelParent)
        {
            Destroy(model.gameObject);
        }
    }
    
    private bool CanAddIngredient(IngredientInfo ingredient)
    {
        var tempIngredients = new List<IngredientInfo>(_ingredients);
        tempIngredients.Add(ingredient);
        
        return FoodManager.Instance.CheckPotIngredients(tempIngredients);
    }

    public void SetWater(bool value)
    {
        waterModel.SetActive(value);
        _hasWater = value;

        if (true)
            AddIngredient(waterIngredient);

    }

    public bool CanCook()
    {
        return _hasWater && FoodManager.Instance.CheckPotRecipe(_ingredients) &&
               _ingredientState < IngredientState.Cooked;
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
      
    }
    private IEnumerator Cooking(float modifier)
    {
        var heatStation = _workstation.GetComponent<HeatStation>();

        while (_ingredientState != IngredientState.Overcooked)
        {
            _workstation.Warning(false);

            while(_ingredientState != IngredientState.Cooked)//Proceso para que se cocine la comida
            {
                _cookingTimer += Time.deltaTime * modifier;
            
                //Actualizamos la UI
                _workstation.UpdateUI(GetCookedProgress());
                heatStation.TurnOnCookSmoke();
                if (_cookingTimer > cookTime)
                {
                    _ingredientState = IngredientState.Cooked;
                    _waterMeshRenderer.material = soupWaterMaterial;
                    //UpdateModel(); AQUI DEBERIAMOS LLAMAR AL NUEVO INGREDIENTE- SE TRASLADA AL ABRIR EL HORNO
                    _cookingTimer = 0;
                    heatStation.TurnOffCookSmoke();
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
                if(_cookingTimer>cookTime*0.1f)
                    heatStation.TurnOnBurntSmoke();
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

    
    public void DishUp(Plate plate)
    {
        if (_rations > 0 && _ingredientState == IngredientState.Cooked && plate.Fill(_ingredients))
        {
            _rations--;

            if (_rations <= 0)
            {
                ResetPot();
            }
            
        }
    }

    private void ResetPot()
    {
        SetWater(false);
        RemoveModel();
        _rations = maxRations;
        _ingredientState = IngredientState.Raw;
        _ingredients.Clear();
        _waterMeshRenderer.material = normalWaterMaterial;

        
        
    }
}
