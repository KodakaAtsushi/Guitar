using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

[RequireComponent(typeof(MaterialUpdater))]
public class SoundListener : MonoBehaviour, IClearTrigger, IDestroyable
{
    FrequencyMatcher matcher;
    MaterialUpdater materialUpdater;
    AnswerNoteView answerView;
    bool isActive = false;
    bool IsActive
    {
        get { return isActive; }
        set
        {
            if(!isActive && value) onEnable.OnNext(default); // falseからtrueになったとき通知
            isActive = value;
        }
    }

    // material
    [SerializeField] Material clearedMat;
    [SerializeField] Material unclearedMat;

    // event
    Observable<Unit> IClearTrigger.OnClear => onClear;
    Subject<Unit> onClear = new();

    public Observable<Unit> OnEnable => onEnable;
    Subject<Unit> onEnable = new();

    CancellationToken ct => destroyCancellationToken;
    bool IDestroyable.IsDestroyed => ct.IsCancellationRequested;

    public void Init(SoundListenerInitData initData)
    {
        // Init fields
        matcher = new FrequencyMatcher(ct, initData.noteNames);
        materialUpdater = GetComponent<MaterialUpdater>();
        answerView = GetComponent<AnswerNoteView>();
        materialUpdater.Init();
        answerView.Init(initData.noteNames);
        
        // Subscribe
        foreach(var soundPlayer in initData.soundPlayers)
        {
            soundPlayer.OnPlaySound.Subscribe(note => {
                if(!IsActive) return;

                matcher.ListenSound(note).Forget();
            }).AddTo(this);
        }

        matcher.OnSoundMatch.Subscribe(isMatch => {
            if(isMatch)
            {
                IsActive = false;
                materialUpdater.SetMaterial(clearedMat);
                answerView.SetTextColor(new Color(0.1f, 0.1f, 0.1f));

                onClear.OnNext(default);
            }
        }).AddTo(this);

        initData.before?.matcher.OnSoundMatch.Subscribe(isMatch => {
            if(!isMatch) return;
            IsActive = true;
        }).AddTo(initData.before);

        // Set initial states
        IsActive = initData.isActive;
        materialUpdater.SetMaterial(unclearedMat);
    }

    void IDestroyable.Destroy()
    {
        Destroy(gameObject);
    }
}
