using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ProgressWidget : MonoBehaviour
{
    [SerializeField] private Image progress;

    private void Start()
    {
        progress.fillAmount = 0;
    }

    public void UpdateUI(float newValue)
    {
        progress.fillAmount = newValue;
    }
}
