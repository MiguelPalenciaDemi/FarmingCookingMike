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
           pot.SetWater(true);


   }

   public override bool CanInteract()
   {
       if (!_objectInWorktop) return false;
       return _objectInWorktop.TryGetComponent(out Plate plate) || _objectInWorktop.TryGetComponent(out Pot pot);
   }
}
