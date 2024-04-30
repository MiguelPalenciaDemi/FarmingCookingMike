using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sink : Workstation
{
   public override void Interact(PlayerInteract player)
    {
        if (_objectInWorktop && _objectInWorktop.TryGetComponent(out Plate plate))
        {
            plate.Clean();
        }
    }
}
