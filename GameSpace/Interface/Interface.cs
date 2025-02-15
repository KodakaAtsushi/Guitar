using R3;
using UnityEngine;

public interface IStringAnchor
{
    
}

public interface IObstacle
{
    
}

public interface ISoundPlayer
{
    Observable<Note> OnPlaySound { get; }
}

public interface IClearTrigger
{
    Observable<Unit> OnClear { get; }
}

public interface IDraggable
{
    bool CanDrag { get; }
    Transform transform { get; }

    void OnPickHandler();
    void OnDragHandler(Vector3 worldPos);
    void OnDropHandler();
    void RetrunParent();
}

public interface IDropPoint
{
    DropResult TryDrop(IDraggable draggable);
}

public interface IDestroyable
{
    bool IsDestroyed { get; }
    void Destroy();
}