using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

[RequireComponent(typeof(PlayerMover), typeof(Collider))]
public class Player : MonoBehaviour, IObstacle, IDestroyable
{
    PlayerMover playerMover;

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
