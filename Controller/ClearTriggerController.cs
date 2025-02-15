using R3;
using Cysharp.Threading.Tasks;

public class ClearTriggerController
{
    public ClearTriggerController(IClearTrigger trigger, SceneLoadRequester requester, PlayerSaveDB saveDB, int stageIndex)
    {
        CompositeDisposable disposables = new();

        trigger.OnClear.SubscribeAwait(async(_, ct) =>
        {
            await UniTask.Delay(1500);  // クリア時1秒待機　遷移が速すぎると、clearしたことがわかりにくくなる
            requester.RequestToLoadNext(delayTime:0);
            saveDB.SaveIsCleared(stageIndex, true);
        }).AddTo(disposables);

        requester.OnRequest.Subscribe(_ =>
        {
            disposables.Dispose();
        }).AddTo(disposables);
    }
}
