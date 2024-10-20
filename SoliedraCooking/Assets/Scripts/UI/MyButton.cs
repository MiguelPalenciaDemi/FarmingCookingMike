using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyButton : Button,INavigableUI
{
    public void Select(bool value)
    {
        var color = targetGraphic.color;
        color.a = value? 1 : 0.5f;
        targetGraphic.color = color;
    }

    public void Interact(int value = 0)
    {
        //Por ahora solo lo usaremos en el slider
        Debug.Log(value +" esto es lo que llegaria al slider");
    }

    public void Press()
    {
        onClick?.Invoke();
    }
}
