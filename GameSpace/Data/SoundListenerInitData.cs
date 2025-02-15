using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundListenerInitData
{
    public readonly IEnumerable<ISoundPlayer> soundPlayers; // guitarのリスト
    public readonly IEnumerable<NoteName> noteNames;
    public readonly bool isActive;
    public readonly SoundListener before;

    public SoundListenerInitData(IEnumerable<ISoundPlayer> soundPlayers, IEnumerable<NoteName> noteNames, bool isActive, SoundListener before)
    {
        this.soundPlayers = soundPlayers;
        this.noteNames = noteNames;
        this.isActive = isActive;
        this.before = before;
    }
}
