using System.Collections;
using System.Collections.Generic;
using R3;
using UnityEngine;

public class StageSelectButtonController
{
    public StageSelectButtonController(IEnumerable<StageSelectButton> buttons, SceneLoadRequester requester)
    {
        foreach(var b in buttons)
        {
            b.OnClick.Subscribe(stageIndex =>
            {
                requester.RequestToLoad(SceneType.InGame, stageIndex);
            }).AddTo(b);
        }
    }
}
