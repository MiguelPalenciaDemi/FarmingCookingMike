using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : Workstation
{

    public override void TakeDrop(PlayerInteract player)
    {
        // ReSharper disable once Unity.PerformanceCriticalCodeNullComparison
        if (player.ObjectPickedUp == null && _objectInWorktop) //Si no tenemos algo en la mano lo podemos coger
        {
            player.TakeObject(_objectInWorktop);
            _objectInWorktop = null;
        }
        else if(player.ObjectPickedUp && !_objectInWorktop)
        {
            player.ObjectPickedUp.TryGetComponent(out Plate plate);
            if (!plate) return;     //Solamente podremos dejar platos aqui.
            
            _objectInWorktop =  player.DropObject();
            SetObjectPosRot();
            if (OrderManager.Instance.DeliverOrder(plate))//Si se completa hacemos que desaparezca
            {
                _objectInWorktop = null;
                Destroy(plate.gameObject);//Meter algun efecto para dar feedback
            }
                

        }
    }
    
    
}
