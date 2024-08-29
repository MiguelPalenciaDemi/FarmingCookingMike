using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyButton : Button
{
    public void Select(bool value)
    {
        var color = targetGraphic.color;
        color.a = value? 1 : 0.5f;
        targetGraphic.color = color;
    }
}
