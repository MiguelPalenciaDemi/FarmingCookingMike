using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;

public class Stove : HeatStation
{
    [SerializeField] private EventReference cookPotSound;
    [SerializeField] private bool isOn;
    private Animator _animator;
    private static readonly int IsOpenAnim = Animator.StringToHash("isOpen");

    private void Awake()
    {
        TryGetComponent(out _animator);
    }
    
    public override bool CanInteract()
    {
        if(!_objectInWorktop) return false; //No hay nada en el.
        if (_objectInWorktop.TryGetComponent(out Ingredient ingredient))
        {
            return ingredient.CanDoAction(CookAction.Cook) || isOn; //Se podrá interactuar si ese ingrediente se puede cocinar o está encendido
        }

        if (!_objectInWorktop.TryGetComponent(out Pot pot)) return false;
        return pot.CanCook() && !isOn;
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    public override void Interact(PlayerInteract player)
    {
        if(!_objectInWorktop) return; //If there isn't any obj to interact we exit
        
        
        //Comprueba si es ingrediente   
        
        if (_objectInWorktop.TryGetComponent(out Ingredient ingredient))
        {
            //Si no se puede cocinar y esta apagado no hace nada
            //Si no se puede cocinar pero esta encendido significa que se esta quemando
            if (!ingredient.CanDoAction(CookAction.Cook) && !isOn) return;
          
            if (!isOn)
            {
                Debug.Log("A cocinar");
                TurnOn(ingredient);
            }
            else
            {
                TurnOff(ingredient);
            }
        }
        else if (_objectInWorktop.TryGetComponent(out Pot pot))
        {
            //if (!ingredient.CanDoAction(CookAction.Cook) && !isOn) return;

            if (pot.CanCook() && !isOn)
            {
                TurnOn(pot);
                
            }
            else
            {
                TurnOff(pot);
            }
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
        _animator.SetTrigger("Cook");
        isOn = true;        
        _cookSoundEventInstance = AudioManager.Instance.PlayLoopEvent3D(cookSound, transform);
        AudioManager.Instance.PlaySoundAtPosition(turnOnSound,transform);

        ShowProgressUI(true,CookAction.Cook);
        ShowInteractUI(false);

    }

    private void TurnOn(Pot pot)
    {
        pot.Cook(this,speedCooking);
        //_animator.SetBool(IsOpenAnim, false);
        isOn = true;       
        _cookSoundEventInstance = AudioManager.Instance.PlayLoopEvent3D(cookPotSound, transform);
        AudioManager.Instance.PlaySoundAtPosition(turnOnSound,transform);

        ShowProgressUI(true,CookAction.Cook);
        ShowInteractUI(false);

    }

    private void TurnOff(Ingredient ingredient)
    {
        Debug.Log("Paramos el cocinado");
        ingredient.StopCook();
        _animator.SetTrigger("Stop");
        isOn = false;      
        AudioManager.Instance.PlaySoundAtPosition(turnOffSound,transform);
        AudioManager.Instance.StopLoopEvent(_cookSoundEventInstance);
        ShowProgressUI(false);
        ShowInteractUI(true,ingredient.gameObject);
        TurnOffBurntSmoke();
        TurnOffCookSmoke();

    }

    private void TurnOff(Pot pot)
    {
        pot.StopCook();
        //_animator.SetBool(IsOpenAnim, true);
        isOn = false;
        AudioManager.Instance.PlaySoundAtPosition(turnOffSound,transform);
        AudioManager.Instance.StopLoopEvent(_cookSoundEventInstance);

        ShowProgressUI(false);
        ShowInteractUI(true,pot.gameObject);
        TurnOffBurntSmoke();
        TurnOffCookSmoke();


    }

    public override void TakeDrop(PlayerInteract player)
    {
        if (!isOn)
        {
            base.TakeDrop(player);

            if (CanPanDisappear(player))
            {
                //_animator.SetTrigger("Take");
                _animator.SetBool("IsPanActive", false);
                Debug.Log("Desaparece la sarten");
            }
            else if (CanPanAppear(player))
            {
                //_animator.SetTrigger("Drop");
                _animator.SetBool("IsPanActive", true);

                Debug.Log("Aparece la sarten");
            }

        }
    }

    public override bool CanTakeDrop()
    {
        return _objectInWorktop && !isOn;
    }

    private bool CanPanAppear(PlayerInteract player)
    {
        
        return !player.ObjectPickedUp && _objectInWorktop && _objectInWorktop.GetComponent<Ingredient>();
    }

    private bool CanPanDisappear(PlayerInteract player)
    {
        // return (player.ObjectPickedUp && player.ObjectPickedUp.GetComponent<Ingredient>()) || (_objectInWorktop &&_objectInWorktop.GetComponent<Plate>());
        return !_objectInWorktop|| (_objectInWorktop &&_objectInWorktop.GetComponent<Plate>());
    }
}
