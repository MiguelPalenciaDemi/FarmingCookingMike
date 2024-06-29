using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class Oven : Workstation
{
    [Header("Audio")] 
    [SerializeField] private EventReference openOven;
    [SerializeField] private EventReference closeOven;
    
    private bool _isOpen = true; //Lo dejamos abierto al inicio
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
        if (!ingredient.CanDoAction(CookAction.Cook) && _isOpen) return;
      
        if (_isOpen)
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
        AudioManager.Instance.PlaySoundAtPosition(closeOven,transform);
        _isOpen = false;        
        
        ShowProgressUI(true,CookAction.Cook);
        ShowInteractUI(false);

    }

    private void TurnOff(Ingredient ingredient)
    {
        ingredient.StopCook();
        _animator.SetBool(IsOpenAnim, true);
        AudioManager.Instance.PlaySoundAtPosition(openOven,transform);
        _isOpen = true;      
        
        ShowProgressUI(false);
        ShowInteractUI(true,ingredient.gameObject);
    }

    public override void TakeDrop(PlayerInteract player)
    {
        if(_isOpen)
            base.TakeDrop(player);
    }
}
