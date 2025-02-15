using System;
using R3;
using UnityEngine;

public class ReloadButtonController
{
    public ReloadButtonController(ReloadButton reloadButton, SceneLoadRequester requester)
    {
        CompositeDisposable disposables = new();

        reloadButton.OnClick.Subscribe(_ =>
        {
            requester.RequestToReload();
        }).AddTo(reloadButton);
    }
}
