using System;
using System.Linq;
using UnityEngine;

public class Note
{
    // 音名と周波数の組みは既定　publicコンストラクタではこの配列の値を参照してインスタンスを作る
    readonly static Note[] notes = new Note[7]{
        new Note(NoteName.C, "ド", 264),
        new Note(NoteName.D, "レ", 297),
        new Note(NoteName.E, "ミ", 330),
        new Note(NoteName.F, "ファ", 352),
        new Note(NoteName.G, "ソ", 396),
        new Note(NoteName.A, "ラ", 440),
        new Note(NoteName.B, "シ", 495)
    };

    readonly public NoteName name;
    readonly public string name_italian;
    readonly public float frequency;

    Note(NoteName name, string name_italian, float frequency)
    {
        this.name = name;
        this.name_italian = name_italian;
        this.frequency = frequency;
    }

    public Note(float frequency)
    {
        Note note = null;

        // notesから、一致するインスタンスを取得(オクターブ違いは区別しない)
        // 周波数比が２倍になったとき、１オクターブ上がる
        foreach(var n in notes)
        {
            var frequencyRatio = frequency / n.frequency;

            float octave = Mathf.Log(frequencyRatio, 2);

            if(octave % 1 == 0)
            {
                note = n;
                break;
            }
        }


        if(note == null)
        {
            name = NoteName.InValid;
            name_italian = "InValid";
        }
        else
        {
            name = note.name;
            name_italian = note.name_italian;
        }
        
        this.frequency = frequency;
    }

    public static string ToItalianName(NoteName noteName)
    {
        var note = notes.First(n => n.name == noteName);

        return note.name_italian;
    }
}

[Serializable]
public enum NoteName
{
    C,
    D,
    E,
    F,
    G,
    A,
    B,
    InValid
}
