using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workstation : MonoBehaviour, IInteractable,ITakeDrop
{
    [SerializeField] protected Transform objectPosition;
    [SerializeField] protected float speedCooking = 1;
    [SerializeField] protected ProgressWidget widgetUI;
    [SerializeField] protected InteractWidget interactUI;
    
    protected GameObject _objectInWorktop;
    public GameObject ObjectInWorktop => _objectInWorktop;


    public virtual void Interact(PlayerInteract player)
    {
      
    }

    public virtual bool CanInteract()
    {
        return false;
    }

    public virtual void ForceStopInteract()
    {
      
    }

    public virtual void TakeDrop(PlayerInteract player)
    {
        if (TryAddIngredient(player)) return;   //Hemos podido añadir ingrediente o emplatar.
        
        // ReSharper disable once Unity.PerformanceCriticalCodeNullComparison
        if (CanTake(player)) //Si no tenemos algo en la mano lo podemos coger
        {
            player.TakeObject(_objectInWorktop);
            _objectInWorktop = null;
            ShowProgressUI(false);
            ShowInteractUI(false);
        }
        else if(CanDrop(player))
        {
            DropObject(player);
            ShowInteractUI(true, _objectInWorktop);
        }

    }

    public virtual bool CanTakeDrop()
    {

        return _objectInWorktop;
    }

    private bool TryAddIngredient(PlayerInteract player)
    {
        if (_objectInWorktop && player.ObjectPickedUp)
        {
             
             
            //Is a Plate and we have an ingredient in our hand?
            if (_objectInWorktop.TryGetComponent(out Plate plate))
            {
                //Ingrediente
                if (player.ObjectPickedUp.TryGetComponent(out Ingredient playerIngredient))
                {
                    if(plate.AddIngredient(playerIngredient.GetIngredientInfo()))
                        Destroy(player.DropObject());
                }
                //POT
                else if (player.ObjectPickedUp.TryGetComponent(out Pot potPicked))
                {
                    potPicked.DishUp(plate);
                }


                return true;
            }
             
             
             
            //Is a Pot and we have an ingredient or plate in our hand?
            if (_objectInWorktop.TryGetComponent(out Pot pot))
            {
                if (player.ObjectPickedUp.TryGetComponent(out Ingredient playerIngredient))
                {
                    if (pot.AddIngredient(playerIngredient.GetIngredientInfo()))
                    {
                        ShowInteractUI(true, _objectInWorktop);
                        Destroy(player.DropObject());
                    }
                }
                //POT
                else if (player.ObjectPickedUp.TryGetComponent(out plate))
                {               
                    pot.DishUp(plate);
                }

                return true;
            }

             
            //Ingrediente en la encimera - Plato en la mano -> juntamos ingredientes y ponemos plato.
            _objectInWorktop.TryGetComponent(out Ingredient ingredient);
            if (ingredient)
            {
                //Plato
                if (player.ObjectPickedUp.TryGetComponent(out plate))
                {
                    if (plate.AddIngredient(ingredient.GetIngredientInfo()))
                    {
                        Destroy(ingredient.gameObject);
                        DropObject(player);
                        ShowInteractUI(false);
                    }

                    return true;
                }
                //Pot
                if (player.ObjectPickedUp.TryGetComponent(out pot))
                {
                    if (pot.AddIngredient(ingredient.GetIngredientInfo()))
                    {
                        Destroy(ingredient.gameObject);
                        DropObject(player);
                        ShowInteractUI(true, _objectInWorktop);
                    }

                    return true;
                }
                
            }
        }

        return false;
    }

    public bool CanTake(PlayerInteract player)
    {
        return player.ObjectPickedUp == null && _objectInWorktop;
    }

    public bool CanDrop(PlayerInteract player)
    {
        return player.ObjectPickedUp && !_objectInWorktop;
    }
    protected void DropObject(PlayerInteract player)
    {
        _objectInWorktop =  player.DropObject();
        SetObjectPosRot();
    }

    protected void SetObjectPosRot()
    {
        _objectInWorktop.transform.parent = objectPosition;
        _objectInWorktop.transform.position = objectPosition.position;
        _objectInWorktop.transform.rotation = Quaternion.identity;
    }

    public void UpdateUI(float progress)
    {
        widgetUI.UpdateUI(progress);
    }

    public void ShowProgressUI(bool progressUIValue, CookAction action= CookAction.None)
    {
        if(!widgetUI) return;
        
        if(action != CookAction.None)
            widgetUI.ChangeIcon(action);
        
        widgetUI.gameObject.SetActive(progressUIValue);
        
        if(!progressUIValue)
            widgetUI.StopWarningSound();
    }

    public void ShowInteractUI(bool value,GameObject ingredient = null, IngredientState state = IngredientState.None)
    {
        if(!interactUI)return;
        
        if(value && ingredient)
            interactUI.Show(ingredient);
        else
            interactUI.Hide();
    }

    public virtual void Warning(bool value)
    {
        widgetUI.SetWarning(value);
    }

    public virtual void RestartWorkstation()
    {
        ShowInteractUI(false);
        ShowProgressUI(false);
        if(_objectInWorktop)
            Destroy(_objectInWorktop);
    }
    
    
}
