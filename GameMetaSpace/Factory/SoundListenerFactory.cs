using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundListenerFactory : MonoBehaviour, IFactory
{
    [SerializeField] GameObject prefab;
    [SerializeField] Transform parent;

    public SoundListener Create(SoundListenerInitData initData)
    {
        var instance = Instantiate(prefab, parent);
        var soundListener = instance.GetComponent<SoundListener>();
        soundListener.Init(initData);

        return soundListener;
    }

    public IEnumerable<SoundListener> Create(IEnumerable<ISoundPlayer> soundPlayers, IEnumerable<IEnumerable<NoteName>> noteNamesList)
    {
        List<SoundListener> soundListeners = new();

        for(int i = 0; i < noteNamesList.Count(); i++)
        {
            var noteNames = noteNamesList.ElementAt(i);
            SoundListenerInitData initData;

            if(i == 0)
            {
                initData = new SoundListenerInitData(soundPlayers, noteNames, true, null);
            }
            else
            {
                var before = soundListeners[i-1];
                initData = new SoundListenerInitData(soundPlayers, noteNames, false, before);
            }

            soundListeners.Add(Create(initData));
        }

        return soundListeners;
    }
}
