using System.Collections;
using System.Collections.Generic;
using R3;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    public Observable<Unit> OnLeftMouseButtonDown => onLeftMouseButtonDown;
    Subject<Unit> onLeftMouseButtonDown = new();

    public Observable<Unit> OnLeftMouseButtonUp => onLeftMouseButtonUp;
    Subject<Unit> onLeftMouseButtonUp = new();

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            onLeftMouseButtonDown.OnNext(default);
        }

        if(Input.GetMouseButtonUp(0))
        {
            onLeftMouseButtonUp.OnNext(default);
        }
    }
}
