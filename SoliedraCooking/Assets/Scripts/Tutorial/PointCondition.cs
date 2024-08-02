using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCondition : MonoBehaviour
{
    private bool _isPlayerIn;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerIn = true;
        }
    }

    public bool IsPlayerIn() => _isPlayerIn;
}
