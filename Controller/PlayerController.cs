using System.Collections.Generic;
using UnityEngine;
using R3;

public class PlayerController
{
    readonly IEnumerable<Player> players;
    readonly PlayerRollObserver rollObserver;

    CompositeDisposable disposables = new();

    public PlayerController(KeyboardInput input, IEnumerable<Player> players, PlayerRollObserver rollObserver)
    {
        this.players = players;
        this.rollObserver = rollObserver;

        input.OnKeyDown.Subscribe(keyCode =>
        {
            Action(keyCode);
        }).AddTo(disposables);

        // playerのいずれかが破棄されたら全て購読解除
        foreach(var player in players)
        {
            player.OnDestroy.Subscribe(_ =>
            {
                disposables.Dispose();
            }).AddTo(disposables);
        }
    }

    void Action(KeyCode keyCode)
    {
        if(rollObserver.IsAnyRolling) return;
        if(DragAndDropper.IsDragging) return;

        switch(keyCode)
        {
            case KeyCode.A:
            case KeyCode.LeftArrow:
                AllMove(Vector3.left);
                break;

            case KeyCode.D:
            case KeyCode.RightArrow:
                AllMove(Vector3.right);
                break;
        }

        if(rollObserver.RemainingCount <= 0) return;

        switch(keyCode)
        {
            case KeyCode.W:
            case KeyCode.UpArrow:
                AllRoll(Vector3.forward);
                break;

            case KeyCode.S:
            case KeyCode.DownArrow:
                AllRoll(Vector3.back);
                break;
        }
    }

    void AllMove(Vector3 moveDir)
    {
        foreach(var player in players)
        {
            player.Move(moveDir);
        }
    }

    void AllRoll(Vector3 rollDir)
    {
        foreach(var player in players)
        {
            player.Roll(rollDir);
        }
    }
}