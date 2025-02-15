using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ThankYouLoader : MonoBehaviour, ILoader
{
    [SerializeField] ReturnButton returnButton;
    [SerializeField] GameObject sceneParent;

    SceneLoadRequester requester;

    public void Init(SceneLoadRequester requester)
    {
        this.requester = requester;
    }

    async UniTask ILoader.Load(object option)
    {
        sceneParent.SetActive(true);

        returnButton.Init();
        new ReturnButtonController(returnButton, requester);

        await UniTask.Delay(0);
    }

    async UniTask ILoader.Unload()
    {
        sceneParent.SetActive(false);

        await UniTask.Delay(0);
    }
}
