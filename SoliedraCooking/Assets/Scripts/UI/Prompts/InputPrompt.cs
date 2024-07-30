using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum InputMode
{
    Keyboard, Gamepad, None
}
public class InputPrompt : MonoBehaviour
{

    [SerializeField] private GameObject gamepadIcon;
    [SerializeField] private GameObject keyboardIcon;

    public void SetIcon(InputMode mode)
    {
        switch (mode)
        {
            case InputMode.Keyboard:
                keyboardIcon.SetActive(true);
                gamepadIcon.SetActive(false);
                break;
            case InputMode.Gamepad:
                gamepadIcon.SetActive(true);
                keyboardIcon.SetActive(false);
                break;
            
        }
    }
}
