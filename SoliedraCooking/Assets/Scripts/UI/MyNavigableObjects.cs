using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class MyNavigableObjects : MonoBehaviour,INavigableUI
{
    
    public virtual void Select(bool value)
    {
        
    }

    public virtual void Interact(int value = 0)
    {
        
    }

    public virtual void Press()
    {
        
    }
}
