using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using R3;

public class VolumeKnobController
{
    public VolumeKnobController(VolumeKnob knob, VolumeChanger changer)
    {
        Init(knob, changer);
    }

    public void Init(VolumeKnob knob, VolumeChanger changer)
    {
        knob.OnValueChange.Subscribe(value =>
        {
            var audioGroup = value.audioGroup;
            var volume = value.volume;
            
            if(volume < 0 || 1 < volume) throw new System.Exception();

            var powValue = Mathf.Pow(volume, (float)1/5);
            var clamped = Mathf.Clamp01(powValue);
            changer.SetVolume(audioGroup, (clamped-1) * 80);
        }).AddTo(knob);
    }
}
