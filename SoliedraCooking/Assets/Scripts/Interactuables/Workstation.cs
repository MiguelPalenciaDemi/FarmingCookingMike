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


    public virtual void Interact(PlayerInteract player)
    {
      
    }
    
    public virtual void ForceStopInteract()
    {
      
    }

    public virtual void TakeDrop(PlayerInteract player)
    {
        // ReSharper disable once Unity.PerformanceCriticalCodeNullComparison
        if (player.ObjectPickedUp == null && _objectInWorktop) //Si no tenemos algo en la mano lo podemos coger
        {
            player.TakeObject(_objectInWorktop);
            _objectInWorktop = null;
            ShowProgressUI(false);
            ShowInteractUI(false);
        }
        else if(player.ObjectPickedUp && !_objectInWorktop)
        {
            DropObject(player);
            ShowInteractUI(true, _objectInWorktop);
        }
       
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
    }

    public void ShowInteractUI(bool value,GameObject ingredient = null, IngredientState state = IngredientState.None)
    {
        if(!interactUI)return;
        
        if(value && ingredient)
            interactUI.Show(ingredient);
        else
            interactUI.Hide();
    }

    public void Warning(bool value)
    {
        widgetUI.SetWarning(value);
    }
    
}
