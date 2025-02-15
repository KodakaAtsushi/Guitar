using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;

internal class LoadController : IDisposable
{
    readonly InGameStateHolder holder;
    readonly DBContainer dBContainer;
    StageDB stageDB => dBContainer.Get<StageDB>();
    readonly Dictionary<SceneType, ILoader> loaderDict;

    Func<UniTask> Unload = async () => await UniTask.Delay(0);
    Func<UniTask> Reload = async () => await UniTask.Delay(0);

    CompositeDisposable disposables = new();

    internal LoadController(DBContainer dBContainer, SceneLoadRequester requester, StageSelectLoader stageSelectLoader, InGameLoader inGameLoader, ThankYouLoader thankYouLoader, InGameStateHolder holder)
    {
        this.holder = holder;
        this.dBContainer = dBContainer;

        loaderDict = new(){
            {SceneType.StageSelect, stageSelectLoader},
            {SceneType.InGame, inGameLoader},
            {SceneType.ThankYou, thankYouLoader}
        };

        requester.OnRequestToLoad.SubscribeAwait(async (value, ct) =>
        {
            var sceneType = value.sceneType;
            var option = value.option;

            await UnloadAndLoad(sceneType, option);
        }).AddTo(disposables);

        requester.OnRequestToLoadNext.SubscribeAwait(async (_, ct) =>
        {
            await LoadNext();
        }).AddTo(disposables);

        requester.OnRequestToReload.SubscribeAwait(async (_, ct) =>
        {
            await ReLoad();
        }).AddTo(disposables);
    }

    async UniTask UnloadAndLoad(SceneType sceneType, object option = null)
    {
        var loader = loaderDict[sceneType];

        await Unload();
        await loader.Load(option);

        CacheCommand(loader, option);
    }

    async UniTask LoadNext()
    {
        (ILoader loader, object option) = GetNextLoader();

        await Unload();
        await loader.Load(option);

        CacheCommand(loader, option);
    }

    async UniTask ReLoad()
    {
        await Unload();
        await Reload();
    }

    // 最後のステージとそれ以外のステージで、遷移先のシーンを分岐
    (ILoader loader, object option) GetNextLoader()
    {
        if(holder.StageIndex + 1 == stageDB.StageCount)
        {
            return (loaderDict[SceneType.ThankYou], default);
        }
        else
        {
            return (loaderDict[SceneType.InGame], holder.StageIndex+1);
        }
    }

    // リロードするときに、最後にロードしたときのoptionを再指定する必要があるため
    // optionを引数に入れてloadする処理そのものを変数に保存
    void CacheCommand(ILoader loader, object option = null)
    {
        Unload = () => loader.Unload();
        Reload = () => loader.Load(option);
    }

    public void Dispose()
    {
        disposables.Dispose();
    }

    // 最初だけ使われる。null分岐を書くと見づらくなったため、これで初期化
    class InitialLoader : ILoader
    {
        async UniTask ILoader.Load(object option)
        {
            await UniTask.Delay(0);
        }

        async UniTask ILoader.Unload()
        {
            await UniTask.Delay(0);
        }
    }
}
