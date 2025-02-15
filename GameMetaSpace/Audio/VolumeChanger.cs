using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeChanger
{
    readonly AudioMixer mixer;

    public VolumeChanger(AudioMixer mixer)
    {
        this.mixer = mixer;
    }

    public void SetVolume(AudioGroup group, float value)
    {
        mixer.SetFloat(group.ToString(), value);
    }

    public float GetVolume(AudioGroup group)
    {
        float value;
        mixer.GetFloat(group.ToString(), out value);
        return value;
    }
}