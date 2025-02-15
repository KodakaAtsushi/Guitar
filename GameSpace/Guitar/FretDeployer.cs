using System;
using R3;
using R3.Triggers;
using UnityEngine;

public class FretDeployer : MonoBehaviour
{
    [SerializeField] Transform anchorParent;
    [SerializeField] Vector3 fretBoxSize = new Vector3(0.3f, 1, 0.5f);

    IStringAnchor[] anchors;
    int splitCount => anchors.Length-1;
    float width;

    // debug draw
    Vector3 boxCenter;

    public void Init(IStringAnchor[] anchors, float width)
    {
        this.anchors = anchors;
        this.width = width;
    }

    public void DeployFret(Fret fret, int index)
    {
        if(anchors[index] != null) throw new Exception();

        anchors[index] = fret;
        SetFretTransform(fret, index);

        // fretがpickされたとき、remove
        CompositeDisposable disposables = new();
        fret.OnPick.Subscribe(_ => {
            RemoveFret(fret);
            disposables.Dispose();
        }).AddTo(disposables);

        if(anchors[index] == null) throw new Exception();
    }

    public void RemoveFret(Fret fret)
    {
        var index = IndexOf(fret);

        if(anchors[index] == null) throw new Exception();

        anchors[index] = null;

        if(anchors[index] != null) throw new Exception();
    }

    void RemoveAllFret()
    {
        for(int i = 1; i < anchors.Length-1; i++)
        {
            var fret = (Fret)anchors[i];

            if(fret == null) continue;

            fret.OnPickHandler(); // add時に購読したremoveFretを呼び出すため
            fret.RetrunParent();
        }
    }

    public bool CanDeploy(Fret fret, out int index)
    {
        var normalizedPos = (fret.transform.position.x + width/2) / width;

        var unclampedIndex = Mathf.RoundToInt(normalizedPos * splitCount);

        index = Mathf.Clamp(unclampedIndex, 1, splitCount-1);

        return IsEmpty(index);
    }

    public void SetAnchors(IStringAnchor[] anchors)
    {
        RemoveAllFret();

        this.anchors = anchors;
    }

    int IndexOf(IStringAnchor anchor)
    {
        int index = 0;

        foreach(var c in anchors)
        {
            if(c == anchor) return index;

            index++;
        }

        throw new Exception("fret is not found");
    }

    void SetFretTransform(Fret fret, int index)
    {
        fret.transform.SetParent(anchorParent);
        var posX = width/splitCount * index - width/2;
        fret.transform.localPosition = new Vector3(posX, 0, 0);
    }

    public bool IsOverlappinig(Fret fret, int index)
    {
        var posX = width/splitCount * index - width/2;
        var localPos = new Vector3(posX, 0, 0);

        boxCenter = anchorParent.transform.position + localPos;

        var colliders = Physics.OverlapBox(boxCenter, fret.GetBoxColliderSize()/2);

        foreach(var c in colliders)
        {
            var obstacle = c.GetComponent<IObstacle>();

            if(obstacle != null && c.gameObject != fret.gameObject)
            {
                Debug.Log($"{c.name} is overlapping");
                return true;
            }
        }
        
        return false;
    }

    bool IsEmpty(int index)
    {
        return anchors[index] == null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(boxCenter, fretBoxSize); // ギズモでボックスを表示
    }
}
