using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class Oven : HeatStation
{
    
    
    private bool _isOpen = true; //Lo dejamos abierto al inicio
    private Animator _animator;
    private static readonly int IsOpenAnim = Animator.StringToHash("isOpen");

    private void Awake()
    {
        TryGetComponent(out _animator);
    }
    
    public override bool CanInteract()
    {
        if(!_objectInWorktop) return false; //No hay nada en el.
        _objectInWorktop.TryGetComponent(out Ingredient ingredient);
        if (!ingredient) return false; //No es un ingrediente
        return ingredient.CanDoAction(CookAction.Cook) || !_isOpen; //Se podrá interactuar si ese ingrediente se puede cocinar o no está abierto
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
        if (!_objectInWorktop) return;
        
        _objectInWorktop.TryGetComponent(out Ingredient ingredient);
        if (!ingredient) return;
        
        TurnOff(ingredient);
    }

    private void TurnOn(Ingredient ingredient)
    {
        ingredient.Cook(this,speedCooking);
        _animator.SetBool(IsOpenAnim, false);
        AudioManager.Instance.PlaySoundAtPosition(turnOnSound,transform);
        _cookSoundEventInstance = AudioManager.Instance.PlayLoopEvent3D(cookSound, transform);
        _isOpen = false;        
        
        ShowProgressUI(true,CookAction.Cook);
        ShowInteractUI(false);

    }

    private void TurnOff(Ingredient ingredient)
    {
        TurnOffBurntSmoke();
        TurnOffCookSmoke();
        ingredient.StopCook();
        _animator.SetBool(IsOpenAnim, true);
        AudioManager.Instance.PlaySoundAtPosition(turnOffSound,transform);
        AudioManager.Instance.StopLoopEvent(_cookSoundEventInstance);
        _isOpen = true;      
        
        ShowProgressUI(false);
        ShowInteractUI(true,ingredient.gameObject);
    }

    public override void TakeDrop(PlayerInteract player)
    {
        if(_isOpen)
            base.TakeDrop(player);
    }

    public override bool CanTakeDrop()
    {
        return _objectInWorktop && _isOpen;
    }

    public override void RestartWorkstation()
    {
        ForceStopInteract();
        base.RestartWorkstation();
    }
}
