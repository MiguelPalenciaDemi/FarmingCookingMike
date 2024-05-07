using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workstation : MonoBehaviour, IInteractable,ITakeDrop
{
    [SerializeField] protected Transform objectPosition;
    [SerializeField] protected float speedCooking = 1;
    [SerializeField] protected ProgressWidget widgetUI;
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
            ShowUI(false);
        }
        else if(player.ObjectPickedUp && !_objectInWorktop)
        {
            DropObject(player);
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

    public void ShowUI(bool value)
    {
        widgetUI.gameObject.SetActive(value);
    }
    
}
