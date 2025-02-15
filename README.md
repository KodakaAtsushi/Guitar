# 概要

Guitarをモチーフにしたゲームです。<br>
ブロックを操作し、指定された音を順番にすべてならすとクリアになります。<br>
企画から実装、またSE・BGMもすべて自主制作しております。<br>
[ここから遊べます](https://unityroom.com/games/guitar_demo)

https://github.com/user-attachments/assets/c375fb3b-eb7c-43d1-b53b-18184cfc51e6

# 使用技術

Unity, C#(R3, UniTask含む)

# 一部ソースコード

キーボード入力によって操作されるブロック（プレイヤー）のクラス

```csharp
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

[RequireComponent(typeof(PlayerMover), typeof(Collider))]
public class Player : MonoBehaviour, IObstacle, IDestroyable
{
    [SerializeField] PlayerMover playerMover;

    // event
    public Observable<bool> OnRoll => playerMover.OnRoll;
    public Observable<Unit> OnDestroy => onDestroy;
    Subject<Unit> onDestroy = new();

    public CancellationToken ct => destroyCancellationToken;
    bool IDestroyable.IsDestroyed => ct.IsCancellationRequested;

    public void Init()
    {
        playerMover = GetComponent<PlayerMover>();
        playerMover.Init(ct);
    }

    public void Move(Vector3 dir)
    {
        if(IsHitObstacle(dir, 3 * Time.deltaTime)) return;

        playerMover.Move(dir);
    }

    public void Roll(Vector3 dir)
    {
        if(IsHitObstacle(dir, 0.5f)) return;

        playerMover.Roll(dir).Forget();
    }

    void IDestroyable.Destroy()
    {
        onDestroy.OnNext(default);
        Destroy(gameObject);
    }

    // 障害物が指定した方向・距離以内にあるかどうかを判定
    bool IsHitObstacle(Vector3 direction, float maxDistance)
    {
        var halfExtents = transform.localScale/2 - Vector3.one*0.01f; // playerがすれ違えるように少し小さく
        var results = Physics.BoxCastAll(transform.position, halfExtents, direction, Quaternion.identity, maxDistance);

        foreach(var r in results)
        {
            var casted = r.collider.GetComponent<IObstacle>();

            if(casted != null && casted != (IObstacle)this) return true;
        }

        return false;
    }
}

```

ブロックとぶつかったときに、Pitchを計算するクラス

```csharp
using R3;
using UnityEngine;
using System;
using System.Collections.Generic;

public class FrequencyCalculator : MonoBehaviour
{
    IReadOnlyList<IStringAnchor> anchors;
    int splitCount => anchors.Count-1;
    float width;

    CompositeDisposable disposables = new();

    public void Init(IReadOnlyList<IStringAnchor> anchors, float width)
    {
        this.anchors = anchors;
        this.width = width;
    }

    public float GetPitch(float pickedPos)
    {
        var normalizedPos = (pickedPos + width/2) / width; // pickedPosの最小値を0にしてからwidthで割る

        int rightIndex = GetNearestIndex_RightSide(normalizedPos);
        int leftIndex = GetNearestIndex_LeftSide(normalizedPos);

        return splitCount / (float)(rightIndex - leftIndex); // 弦長比の逆数がPitch
    }

    public void SetAnchors(IStringAnchor[] anchors)
    {
        this.anchors = anchors;
    }

    int GetNearestIndex_LeftSide(float normalizedPos)
    {
        // 一番近い左側のAnchorを取得
        for(int i = Mathf.FloorToInt(normalizedPos*splitCount); i > -1; i--)
        {
            var anchor = anchors[i];

            if(anchor == null) continue;

            return i; // left index
        }

        throw new Exception("左端のAnchorがない");
    }

    int GetNearestIndex_RightSide(float normalizedPos)
    {
        // 一番近い右側のAnchorを取得
        for(int i = Mathf.CeilToInt(normalizedPos*splitCount); i < anchors.Count; i++)
        {
            var anchor = anchors[i];

            if(anchor == null) continue;

            return i; // right index;
        }

        throw new Exception("右端のAnchorがない");
    }

    void OnDestory()
    {
        disposables.Dispose();
    }
}

```

ゲーム内で正解判定に使われる音を表すクラス

```csharp

using System;
using System.Collections.Generic;
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

```
