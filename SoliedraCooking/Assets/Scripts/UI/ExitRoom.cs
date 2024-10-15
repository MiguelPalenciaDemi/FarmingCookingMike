using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitRoom : MonoBehaviour
{
    [SerializeField] private InitialTransitionController transitionController;
    [SerializeField] private int indexScene;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        StartCoroutine(transitionController.EndWithAction( () =>{ SceneManager.LoadScene(indexScene); }));
    }
}
