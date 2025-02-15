using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    List<AudioSource> audioSources = new();

    [SerializeField] float decay = 10;
    [SerializeField] float volume = 2;

    static AudioClip clip_264Hz;

    public void Init()
    {
        if(clip_264Hz == null) clip_264Hz = CreateAudioClip(264); // 264Hzのsine波

        AddAudioSource();
    }

    public void PlaySound(float pitch)
    {
        AudioSource audioSource = audioSources.FirstOrDefault(aS => aS.isPlaying == false);

        if(audioSource == null)
        {
            audioSource = AddAudioSource();
        }

        audioSource.pitch = pitch;
        audioSource.Play();
    }

    // audioSourcesに追加
    AudioSource AddAudioSource()
    {
        var audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.spatialBlend = 1;
        audioSource.clip = clip_264Hz;

        audioSources.Add(audioSource);

        return audioSource;
    }

    AudioClip CreateAudioClip(float frequency)
    {
        float[] samplingHights = new float[96000];

        for(int i = 0; i < samplingHights.Length; i++)
        {
            var sample = Mathf.Sin(2 * Mathf.PI * frequency * i / 48000);

            var decayAmount = Mathf.Exp(-(float)i / 96000 * decay); // 減衰の係数

            samplingHights[i] = sample * decayAmount * volume;
        }

        var clip =  AudioClip.Create($"clip_{frequency}Hz", 96000, 1, 48000, false);
        clip.SetData(samplingHights, 0);

        return clip;
    }
}
