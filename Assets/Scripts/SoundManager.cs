using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum soundTriggers {
Murder,
MurderStopped,
MurderNew,
UIClick
    
}

[Serializable]
public class SoundEffect
{
    [SerializeField] public soundTriggers Trigger;
    [SerializeField] public AudioClip SFX;
    [HideInInspector] public AudioSource source;
    [SerializeField] public float Pitch;
    [SerializeField] public float Volume;

}

public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundEffect[] SFXPubliclist;
    [SerializeField] public static SoundEffect[] SFXlist;
    void Start()
    {
        SFXlist = SFXPubliclist;

        foreach (SoundEffect effect in SFXlist) 
        {
            InitializeSound(effect);
        }
    }

    void InitializeSound(SoundEffect SFX)
    {
        AudioSource AS = gameObject.AddComponent<AudioSource>();
        AS.clip = SFX.SFX;
        AS.pitch = SFX.Pitch; 
        AS.volume = SFX.Volume;
        AS.enabled = true;
        SFX.source = AS;
        AS.loop = false;
        AS.playOnAwake = false;
    }

    public static void playSound(soundTriggers tirgger)
    {
        foreach(SoundEffect sfx in SFXlist)
        {
            if (sfx.Trigger == tirgger) sfx.source.Play();
        }
    }

    
}
