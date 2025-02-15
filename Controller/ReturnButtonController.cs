using System.Collections;
using System.Collections.Generic;
using R3;
using UnityEngine;

public class ReturnButtonController
{
    public ReturnButtonController(ReturnButton button, SceneLoadRequester requester)
    {
        CompositeDisposable disposables = new();

        button.OnClick.Subscribe(_ =>
        {
            requester.RequestToLoad(SceneType.StageSelect);
        }).AddTo(button);
    }
}
