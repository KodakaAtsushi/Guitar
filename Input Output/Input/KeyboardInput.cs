using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    public Observable<KeyCode> OnKeyDown => onKeyDown;
    Subject<KeyCode> onKeyDown = new();

    void Update()
    {
        if(Input.anyKey)
        {
            var keyList = (IEnumerable<KeyCode>)Enum.GetValues(typeof(KeyCode));

            var downedKey = keyList.First(key => Input.GetKey(key));
            onKeyDown.OnNext(downedKey);
        }
    }
}
