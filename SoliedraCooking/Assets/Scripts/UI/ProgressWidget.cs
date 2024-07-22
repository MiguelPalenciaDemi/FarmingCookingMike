using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ProgressWidget : MonoBehaviour
{
    [SerializeField] private Image progress;
    [SerializeField] private List<InteractIcon> icons;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color warningColor;
    [Header("Audio")] 
    [SerializeField] private EventReference warningSound;
    private EventInstance _warningEventSoundInstance;
    private void Start()
    {
        progress.fillAmount = 0;
    }

    public void UpdateUI(float newValue)
    {
        progress.fillAmount = newValue;
    }

    public void ChangeIcon(CookAction type)
    {
        Hide();
        icons.Find(x =>x.action == type).icon.SetActive(true);
    }
    
    public void Hide()
    {
        //Ocultamos todos los iconos
        foreach (var item in icons)
            item.icon.SetActive(false);
    }
    
    public void SetWarning(bool isWarning)
    {
        progress.color = isWarning? warningColor: normalColor;
        
        // if (isWarning)
        // {
        //     _warningEventSoundInstance = AudioManager.Instance.PlayLoopEvent3D(warningSound, transform);
        // }
        // else
        // {
        //     if (_warningEventSoundInstance.isValid())
        //     {
        //         AudioManager.Instance.StopLoopEvent(_warningEventSoundInstance);
        //     }
        // }
    }
}
