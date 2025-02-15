using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using R3;

public class DragAndDropper : MonoBehaviour
{
    public static bool IsDragging { get; private set; } = false;

    Vector3 mouseOffSet;

    RayCaster rayCaster;

    public void Init(MouseInput mouseInput, RayCaster rayCaster)
    {
        this.rayCaster = rayCaster;

        mouseInput.OnLeftMouseButtonDown.Subscribe(_ =>
        {
            CompositeDisposable disposables = new();

            // draggable取得　ないまたはdrag不可ならreturn
            IDraggable draggable;

            if(!rayCaster.RayHit(out draggable)) return;
            if(!draggable.CanDrag) return;

            // Drag開始
            BeginDrag(draggable);
            IsDragging = true;

            // 毎フレームDrag()を呼び出す
            Observable.EveryUpdate().Subscribe(_ =>
            {
                Drag(draggable);
            }).AddTo(disposables);

            // 離されたら、Drag()を毎フレーム呼び出すのをやめる
            mouseInput.OnLeftMouseButtonUp.Subscribe(_ =>
            {
                EndDrag(draggable);
                IsDragging = false;

                disposables.Dispose();
            }).AddTo(disposables);

        }).AddTo(mouseInput);
    }

    void BeginDrag(IDraggable draggable)
    {
        draggable.OnPickHandler();

        mouseOffSet = draggable.transform.position - rayCaster.GetHitGroundPos();
    }

    void Drag(IDraggable draggable)
    {
        if(draggable == null) throw new Exception("draggable is nul");

        draggable.OnDragHandler(rayCaster.GetHitGroundPos() + mouseOffSet);
    }

    void EndDrag(IDraggable draggable)
    {
        if(draggable == null) throw new Exception("draggable is nul");

        if(rayCaster.RayHit(out IDropPoint dropPoint))
        {
            var result = dropPoint.TryDrop(draggable);

            switch(result)
            {
                case DropResult.Success:
                    draggable.OnDropHandler();
                    break;

                case DropResult.Failure:
                    draggable.RetrunParent();
                    break;

                default:
                    throw new NotImplementedException();
            }
        }
        else
        {
            draggable.RetrunParent();
        }

        mouseOffSet = Vector3.zero;
    }
}

