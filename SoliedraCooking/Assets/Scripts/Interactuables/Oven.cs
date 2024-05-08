using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : Workstation
{
    [SerializeField] private bool isOpen;
    private Animator _animator;
    private static readonly int IsOpenAnim = Animator.StringToHash("isOpen");

    private void Awake()
    {
        TryGetComponent(out _animator);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public override void Interact(PlayerInteract player)
    {
        if(!_objectInWorktop) return; //If there isn't any obj to interact we exit
        
        //First get the ingredient   
        _objectInWorktop.TryGetComponent(out Ingredient ingredient);
        if (!ingredient) return;
      
        if (isOpen)
        {
            TurnOn(ingredient);
        }
        else
        {
            TurnOff(ingredient);
        }
        
        
    }

    public override void ForceStopInteract()
    {
        _objectInWorktop.TryGetComponent(out Ingredient ingredient);
        if (!ingredient) return;
        
        TurnOff(ingredient);
    }

    private void TurnOn(Ingredient ingredient)
    {
        ingredient.Cook(this,speedCooking);
        _animator.SetBool(IsOpenAnim, false);
        isOpen = false;        
        //ShowUI(true, IngredientState.Cooked);
        ShowProgressUI(true,IngredientState.Cooked);
        ShowInteractUI(false);

    }

    private void TurnOff(Ingredient ingredient)
    {
        ingredient.StopCook();
        _animator.SetBool(IsOpenAnim, true);
        isOpen = true;      
        //ShowUI(true, IngredientState.Cooked);
        ShowProgressUI(false);
        ShowInteractUI(true,ingredient.gameObject);
        //interactUI.Show(ingredient.gameObject);

    }

    public override void TakeDrop(PlayerInteract player)
    {
        if(isOpen)
            base.TakeDrop(player);
    }
}
