using System;
using System.Collections;
using System.Collections.Generic;
using FMOD;
using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    //private StudioEventEmitter _emitter;
    private static AudioManager _instance;

    public static AudioManager Instance => _instance;

    private void Awake()
    {
        if (_instance)
            Destroy(this);
        else
            _instance = this;
        
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySoundAtPosition(EventReference eventReference, Transform transformPosition)
    {
        
        var instance = RuntimeManager.CreateInstance(eventReference);
        instance.set3DAttributes(transformPosition.To3DAttributes());
        instance.start();
        instance.release();
    }

    
    
}
