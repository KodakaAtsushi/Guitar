using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

public class PlayerRollObserver
{
    // event
    public Observable<int> OnChangeRemainingCount => onChangeRemainingCount;
    Subject<int> onChangeRemainingCount = new();

    public bool IsAnyRolling { get; private set; }
    public int RemainingCount { get; private set; }

    readonly CancellationToken ct;

    bool isWaiting = false;

    public PlayerRollObserver(IEnumerable<Player> players, int rollLimit)
    {
        RemainingCount = rollLimit;

        ct = players.ElementAt(0).destroyCancellationToken;

        /*
            1f以内に複数Playerが回転したとき、2番目以降のPlayerからのOnRoll通知は
            isWaitingではじかれる
        */
        foreach(var player in players)
        {
            player.OnRoll.SubscribeAwait(async (value, ct) => {
                if(isWaiting) return;
                isWaiting = true;

                await UniTask.Yield();

                IsAnyRolling = value;

                if(value)
                {
                    RemainingCount--;
                    onChangeRemainingCount.OnNext(RemainingCount);
                }

                isWaiting = false;
            }).AddTo(ct);
        }
    }
}
