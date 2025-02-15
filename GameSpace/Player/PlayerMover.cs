using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3;
    [SerializeField] float rotateSpeed = 180;
    
    bool isRolling = false;

    // event
    public Observable<bool> OnRoll => onRoll;
    Subject<bool> onRoll = new();

    CancellationToken ct;

    public void Init(CancellationToken ct)
    {
        this.ct = ct;
    }

    public void Move(Vector3 moveDir)
    {
        if(isRolling) return;
        if(moveDir != Vector3.right && moveDir != Vector3.left) throw new ArgumentException();

        var moveAmount = moveDir * moveSpeed * Time.deltaTime;
        transform.Translate(moveAmount, Space.Self);
    }

    async public UniTask Roll(Vector3 rollDir)
    {
        if(isRolling) return;
        if(rollDir != Vector3.forward && rollDir != Vector3.back) throw new ArgumentException();

        isRolling = true;
        onRoll.OnNext(isRolling);

        Vector3 point = new Vector3(0, -0.5f, 0.5f * rollDir.z) + transform.position;
        Vector3 axis = Vector3.right;

        float rotatedAngle = 0;

        while(Mathf.Abs(rotatedAngle) < 90)
        {
            var thisFrameAngle = rollDir.z * rotateSpeed * Time.deltaTime;
            transform.RotateAround(point, axis, thisFrameAngle);

            rotatedAngle += thisFrameAngle;

            await UniTask.Yield(ct);
        }

        AdjustTransform();

        isRolling = false;
        onRoll.OnNext(isRolling);
    }

    void AdjustTransform()
    {
        transform.position = new Vector3(
            transform.position.x, 0.5f, Mathf.Round(transform.position.z));

        transform.rotation = Quaternion.Euler(Vector3.zero); // 向きは考慮しない
    }
}
