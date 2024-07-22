using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialoguePopUp : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI textMessage;
    [SerializeField] private GameObject tutorialSignal;
    private Transform cameraTransform;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    public void SetText(string newMessage)
    {
        textMessage.text = newMessage;
    }

    public void Show(bool value)
    {
        panel.SetActive(value);
        tutorialSignal.SetActive(!value);
    }

    private void Update()
    {
        var rotation = ClampRotation();
        tutorialSignal.transform.rotation = rotation;
        panel.transform.rotation = rotation;
    }

    private Quaternion CalcultateRotation()
    {
        return Quaternion.LookRotation(transform.position - cameraTransform.position); //Buscamos la rotación que enfrenta a la camara.
    }

    private Quaternion ClampRotation()
    {
        var rotation = CalcultateRotation();
        
        // rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, -Mathf.Clamp(rotation.eulerAngles.y-180, -10, 10),
        //     rotation.eulerAngles.z);
        if(rotation.eulerAngles.y>180)
            rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, Mathf.Clamp(rotation.eulerAngles.y, 350, 360),
                rotation.eulerAngles.z);
        else
            rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, Mathf.Clamp(rotation.eulerAngles.y, 0, 10),
                rotation.eulerAngles.z);
        
        return rotation;

    }
}
