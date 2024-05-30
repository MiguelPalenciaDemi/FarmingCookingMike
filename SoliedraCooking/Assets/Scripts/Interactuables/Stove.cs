using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Stove : Workstation
{
        [SerializeField] private bool isOn;
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
            //_animator.SetBool(IsOpenAnim, false);
            isOn = true;        
            
            ShowProgressUI(true,CookAction.Cook);
            ShowInteractUI(false);
    
        }
        
        private void TurnOn(Pot pot)
        {
            pot.Cook(this,speedCooking);
            //_animator.SetBool(IsOpenAnim, false);
            isOn = true;        
            
            ShowProgressUI(true,CookAction.Cook);
            ShowInteractUI(false);
    
        }
    
        private void TurnOff(Ingredient ingredient)
        {
            Debug.Log("Paramos el cocinado");
            ingredient.StopCook();
            //_animator.SetBool(IsOpenAnim, true);
            isOn = false;      
            
            ShowProgressUI(false);
            ShowInteractUI(true,ingredient.gameObject);
        }
        
        private void TurnOff(Pot pot)
        {
            pot.StopCook();
            //_animator.SetBool(IsOpenAnim, true);
            isOn = false;      
            
            ShowProgressUI(false);
            ShowInteractUI(true,pot.gameObject);
        }
    
        public override void TakeDrop(PlayerInteract player)
        {
            if(!isOn)
                base.TakeDrop(player);
        }
}
