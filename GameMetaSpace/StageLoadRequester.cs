using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

public class SceneLoadRequester
{
    public Observable<Unit> OnRequest => onRequest;
    Subject<Unit> onRequest = new();

    public Observable<(SceneType sceneType, object option)> OnRequestToLoad => onRequestToLoad;
    Subject<(SceneType sceneType, object option)> onRequestToLoad = new();

    public Observable<object> OnRequestToReload => onRequestToReload;
    Subject<object> onRequestToReload = new();

    public Observable<object> OnRequestToLoadNext => onRequestToLoadNext;
    Subject<object> onRequestToLoadNext = new();

    CompositeDisposable disposables = new();

    public SceneLoadRequester()
    {
        OnRequestToLoad.Subscribe(_ =>
        {
            onRequest.OnNext(default);
        }).AddTo(disposables);

        onRequestToReload.Subscribe(_ =>
        {
            onRequest.OnNext(default);
        }).AddTo(disposables);

        onRequestToLoadNext.Subscribe(_ =>
        {
            onRequest.OnNext(default);
        }).AddTo(disposables);
    }

    async public void RequestToLoad(SceneType sceneType, object option = null, int delayTime = 150)
    {
        await UniTask.Delay(delayTime);
        onRequestToLoad.OnNext((sceneType, option));
    }

    async public void RequestToReload(int delayTime = 150)
    {
        await UniTask.Delay(delayTime);
        onRequestToReload.OnNext(default);
    }

    async public void RequestToLoadNext(int delayTime = 150)
    {
        await UniTask.Delay(delayTime);
        onRequestToLoadNext.OnNext(default);
    }
}

public enum SceneType
{
    StageSelect,
    InGame,
    ThankYou
}