using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sink : Workstation
{
   public override void Interact(PlayerInteract player)
   {
       if (!_objectInWorktop) return;
       
       
       if(_objectInWorktop.TryGetComponent(out Plate plate))
           plate.Clean();
       else if(_objectInWorktop.TryGetComponent(out Pot pot))
           pot.AddWater();


   }
}
