using System.Collections;
using System.Collections.Generic;
using R3;
using UnityEngine;

[RequireComponent(typeof(GameSpaceButton))]
public class ReloadButton : MonoBehaviour
{
    GameSpaceButton button;

    public Observable<Unit> OnClick => button.OnClick;

    public void Init()
    {
        button = GetComponent<GameSpaceButton>();
        button.Init();
    }
}
