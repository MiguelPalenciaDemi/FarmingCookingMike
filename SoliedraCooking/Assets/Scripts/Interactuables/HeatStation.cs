using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class HeatStation : Workstation
{
    [SerializeField] private ParticleSystem cookSmoke;
    [SerializeField] private ParticleSystem burntSmoke;
    [Header("Audio")] 
    [SerializeField] protected EventReference turnOnSound;
    [SerializeField] protected EventReference turnOffSound;
    [SerializeField] protected EventReference cookSound;
    [SerializeField] protected EventReference readySound;
    protected EventInstance _cookSoundEventInstance; 
    public void ReadyNotification()
    {
        AudioManager.Instance.PlaySoundAtPosition(readySound,transform);
    }
    
    public override void Interact(PlayerInteract player)
    {
      base.Interact(player);
    }
    
    public override void ForceStopInteract()
    {
      base.ForceStopInteract();
    }
    public override void Warning(bool value)
    {
        widgetUI.SetWarning(value);
    }

    public void TurnOnBurntSmoke()
    {
        if(!burntSmoke.isPlaying)
            burntSmoke.Play();
    }

    protected void TurnOffBurntSmoke()
    {
        burntSmoke.Stop();
    }
    
    public void TurnOnCookSmoke()
    {
        if(!cookSmoke.isPlaying)
            cookSmoke.Play();
    }

    public void TurnOffCookSmoke()
    {
        cookSmoke.Stop();
    }
    
}
