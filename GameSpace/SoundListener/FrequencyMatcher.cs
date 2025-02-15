using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using System.Linq;
using UnityEngine;

public class FrequencyMatcher
{
    readonly IEnumerable<NoteName> answerNoteNames;

    List<Note> listenedList = new();

    bool isReserved = false;

    // event
    public Observable<bool> OnSoundMatch => onSoundMatch;
    readonly Subject<bool> onSoundMatch = new();

    CancellationToken ct;

    public FrequencyMatcher(CancellationToken ct, IEnumerable<NoteName> noteNames)
    {
        this.ct = ct;
        answerNoteNames = noteNames;
    }

    /*
        同時に音が鳴ったことを判定したい
        音を受け取った後、3ms待機状態になり、その間になったすべての音と、answerとを比較する
    */
    async public UniTask ListenSound(Note note)
    {
        listenedList.Add(note);

        if(isReserved) return;

        isReserved = true;

        // 3f待機
        await UniTask.Delay(3, cancellationToken: ct);

        // frequency を音名に変換
        var listenedNoteNames = listenedList.Select(note => note.name);

        // 正解判定
        onSoundMatch.OnNext(IsMatchAll(listenedNoteNames));

        // 事後処理
        listenedList = new();
        isReserved = false;
    }

    /*
        
    */
    bool IsMatchAll(IEnumerable<NoteName> noteNames)
    {
        var noteNames_copy = new List<NoteName>(noteNames);

        // answerをすべて含むか確認、copyから削除
        foreach(var answer in answerNoteNames)
        {
            if(noteNames_copy.Contains(answer))
            {
                noteNames_copy.Remove(answer);
                continue;
            }

            return false;
        }

        // answerにないnoteNameが残っているかを確認
        foreach(var remained in noteNames_copy)
        {
            if(answerNoteNames.Contains(remained))
            {
                continue;
            }

            return false;
        }

        return true;
    }
}
