using System;
using System.Collections;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using UnityEngine;
using FMODUnity;
using Debug = UnityEngine.Debug;
using STOP_MODE = FMOD.Studio.STOP_MODE;

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
        
        var eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstance.set3DAttributes(transformPosition.To3DAttributes());
        eventInstance.start();
        eventInstance.release();
    }

    public EventInstance PlayLoopEvent3D(EventReference eventReference, Transform transformPosition)
    {
        if (eventReference.IsNull) return default;
        
        var eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstance.set3DAttributes(transformPosition.To3DAttributes());
        eventInstance.start();
        Debug.Log("hey");
        return eventInstance;
    }

    public void StopLoopEvent(EventInstance eventInstance)
    {
        if(!eventInstance.isValid()) return;
        
        eventInstance.stop(STOP_MODE.ALLOWFADEOUT);//Permite que actue el AHR
        eventInstance.release();
    }

    
    
}
