using System.Collections;
using System.Collections.Generic;
using R3;
using UnityEngine;

public class Fret : MonoBehaviour, IStringAnchor, IObstacle, IDraggable
{
    bool canDrag;
    bool IDraggable.CanDrag => canDrag;

    public Observable<Unit> OnPick => onPick;
    public Observable<Unit> OnDrop => onDrop;
    Subject<Unit> onPick = new();
    Subject<Unit> onDrop = new();

    FretPool pool; // fret poolと fret は相互参照

    Vector3 boxColliderSize;

    [SerializeField] Material unmoveMat;

    public void Init(FretPool fretPool, bool canDrag = true, bool canRepick = false)
    {
        pool = fretPool;

        var boxCollider = GetComponent<BoxCollider>();
        boxColliderSize = Vector3.Scale(boxCollider.size, transform.localScale);

        this.canDrag = canDrag;

        if(!canDrag)
        {
            SetUnmoveMat();
        }

        // 一度配置すると再配置できない
        if(!canRepick)
        {
            OnDrop.Subscribe(_ => {
                this.canDrag = false;
                SetUnmoveMat();
            }).AddTo(this);
        }
    }

    public Vector3 GetBoxColliderSize()
    {
        return boxColliderSize;
    }

    public void RetrunParent()
    {
        pool.PoolFret(this);
    }

    void SetUnmoveMat()
    {
        var mr = GetComponent<MeshRenderer>();
        mr.material = unmoveMat;
    }

    public void OnPickHandler()
    {
        onPick.OnNext(default);
    }

    public void OnDragHandler(Vector3 worldPos)
    {
        transform.position = worldPos;
    }

    public void OnDropHandler()
    {
        onDrop.OnNext(default);
    }
}
