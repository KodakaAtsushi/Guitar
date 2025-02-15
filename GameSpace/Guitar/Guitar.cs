using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

[RequireComponent(typeof(FrequencyCalculator), typeof(FretDeployer), typeof(BoxCollider))]
[RequireComponent(typeof(SplitLineSetter), typeof(SoundPlayer))]
public class Guitar : MonoBehaviour, ISoundPlayer, IDropPoint, IDestroyable
{
    // input components
    GameSpaceButton button;
    ColliderTrigger stringCollider;

    // model components
    FretDeployer fretDeployer;
    FrequencyCalculator frequencyCalculator;
    SplitLineSetter splitLineSetter;
    SoundPlayer soundPlayer;
    SplitCountChanger splitCountChanger;

    // fields
    [SerializeField] Nut leftEndNut;
    [SerializeField] Nut rightEndNut;

    IStringAnchor[] anchors;
    IStringAnchor[] Anchors
    {
        get { return anchors; }
        set
        {
            anchors = value;
            frequencyCalculator.SetAnchors(anchors);
            fretDeployer.SetAnchors(anchors);
            splitLineSetter.SetSplitLine(anchors.Length-1);
        }
    }

    bool isInitialized = false;

    // event
    Observable<Note> ISoundPlayer.OnPlaySound => onPlaySound;
    Subject<Note> onPlaySound = new();

    CancellationToken ct => destroyCancellationToken;
    bool IDestroyable.IsDestroyed => ct.IsCancellationRequested;

    async public UniTask Init(GuitarInitData data, List<Fret> frets)
    {
        transform.localPosition = data.Position;
        anchors = CreateAnchors(data.SplitCount);

        var width = 10; // guitarの幅

        // init components =======================
        button = GetComponentInChildren<GameSpaceButton>();
        stringCollider = GetComponentInChildren<ColliderTrigger>();
        frequencyCalculator = GetComponent<FrequencyCalculator>();
        fretDeployer = GetComponent<FretDeployer>();
        splitLineSetter = GetComponent<SplitLineSetter>();
        soundPlayer = GetComponent<SoundPlayer>();
        splitCountChanger = GetComponent<SplitCountChanger>();

        button.Init();
        frequencyCalculator.Init(anchors, width);
        fretDeployer.Init(anchors, width);
        splitLineSetter.Init(data.SplitCount, width);
        soundPlayer.Init();
        splitCountChanger.Init(data.SplitCount);
        
        // subscribe ==============================
        button.OnClick.Subscribe(_ =>{
            splitCountChanger.IncrementIndex();
        }).AddTo(this);

        stringCollider.OnEnter.Subscribe(collider => {
            if(!isInitialized) return;

            var pickedPos = collider.transform.position.x;
            var pitch = frequencyCalculator.GetPitch(pickedPos);
            soundPlayer.PlaySound(pitch);

            onPlaySound.OnNext(new Note(264 * pitch));
        }).AddTo(this);

        splitCountChanger.OnCountChange.Subscribe(splitCount => {
            Anchors = CreateAnchors(splitCount);
        }).AddTo(this);

        // other initalize process ================
        // deploy frets
        for(int i = 0; i < frets.Count; i++)
        {
            var fret = frets[i];
            var index = data.FretIndices[i];

            fretDeployer.DeployFret(fret, index);
        }

        // set activation button
        button.gameObject.SetActive(data.IsButtonEnable);

        // enable soundPlayer 初期化時に音がならないように
        await UniTask.DelayFrame(3, cancellationToken:ct);
        isInitialized = true;
    }

    void IDestroyable.Destroy()
    {
        Destroy(gameObject);
    }

    DropResult IDropPoint.TryDrop(IDraggable draggable)
    {
        if(draggable is Fret fret && fretDeployer.CanDeploy(fret, out int index) && !fretDeployer.IsOverlappinig(fret, index))
        {
            fretDeployer.DeployFret(fret, index);
            button.SetDisable();
            return DropResult.Success;
        }

        return DropResult.Failure;
    }

    IStringAnchor[] CreateAnchors(int splitCount)
    {
        var anchors = new IStringAnchor[splitCount+1];
        anchors[0] = leftEndNut;
        anchors[anchors.Length-1] = rightEndNut;

        return anchors;
    }
}