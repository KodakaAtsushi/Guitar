using System.Collections.Generic;
using UnityEngine;
using R3;
using Cysharp.Threading.Tasks;
using System.Linq;
using System;

internal class InGameLoader : MonoBehaviour, ILoader
{
    // Containers
    DBContainer dBContainer;
    FactoryContainer factoryContainer;
    UIContainer uIContainer;

    // Priavte propertys
    StageDB stageDB => dBContainer.Get<StageDB>();
    PlayerSaveDB saveDB => dBContainer.Get<PlayerSaveDB>();

    PlayerFactory playerFactory => factoryContainer.Get<PlayerFactory>();
    GuitarFactory guitarFactory => factoryContainer.Get<GuitarFactory>();
    SoundListenerFactory soundListenerFactory => factoryContainer.Get<SoundListenerFactory>();
    FretFactory fretFactory => factoryContainer.Get<FretFactory>();

    KeyboardInput keyboardInput => uIContainer.Get<KeyboardInput>();

    // Scene Objects
    [SerializeField] ReloadButton reloadButton;
    [SerializeField] ReturnButton returnButton;
    [SerializeField] RollCountLight rollCountLight;
    [SerializeField] FretPool fretPool;
    [SerializeField] Arrow arrow;

    [SerializeField] GameObject sceneParent;

    // others
    SceneLoadRequester requester;
    InGameStateHolder holder;
    List<IDestroyable> destroyables = new();

    public void Init(DBContainer dBContainer, FactoryContainer factoryContainer, UIContainer uIContainer, SceneLoadRequester requester, InGameStateHolder holder)
    {
        this.dBContainer = dBContainer;
        this.factoryContainer = factoryContainer;
        this.uIContainer = uIContainer;
        this.requester = requester;
        this.holder = holder;

        reloadButton.Init();
        returnButton.Init();
        new ReloadButtonController(reloadButton, requester);
        new ReturnButtonController(returnButton, requester);

        destroyables = new(); // ここで初期化しないとなぜかnullエラーが出た
    }

    async UniTask ILoader.Load(object option)
    {
        await LoadStage((int)option);
    }

    async UniTask ILoader.Unload()
    {
        await DestroyStageObject();
        sceneParent.SetActive(false);
    }

    async internal UniTask LoadStage(int stageIndex)
    {
        holder.SetStageIndex(stageIndex);

        // activate
        sceneParent.SetActive(true);

        // index更新　stage取得
        var stage = stageDB.Stages[stageIndex];

        // Init players
        var players = playerFactory.Create(stage.PlayerPositions);
        AddDestroyable(players);

        var rollObserver = new PlayerRollObserver(players, stage.RollLimit);

        // Init guitar
        var guitars = guitarFactory.Create(stage.Guitars);
        AddDestroyable(guitars);

        // Init soundListeners
        var soundListeners = soundListenerFactory.Create(guitars, stage.NoteNamesList);
        ObjectDeployer.Deploy(soundListeners, new Vector3(0, 0, 1.1f));
        AddDestroyable(soundListeners);

        // init other objects
        fretPool.Init(fretFactory.Create(Enumerable.Repeat(true, stage.FretCount)));
        rollCountLight.Init(stage.RollLimit, rollObserver);

        // Init Controllers
        var clearTrigger = soundListeners.ElementAt(soundListeners.Count()-1);
        new ArrowController(arrow, soundListeners);
        new PlayerController(keyboardInput, players, rollObserver);
        new ClearTriggerController(clearTrigger, requester, saveDB, stageIndex);

        await UniTask.Delay(0);
    }

    // destroyablesに登録していたオブジェクトをすべて破壊
    async UniTask DestroyStageObject()
    {
        foreach(var destroyable in destroyables)
        {
            destroyable.Destroy();
        }

        await UniTask.WaitUntil(() => destroyables.All(d => d.IsDestroyed));

        destroyables = new();
    }

    // unload時破壊してほしいオブジェクトを追加
    void AddDestroyable(IEnumerable<IDestroyable> destroyables)
    {
        foreach(var d in destroyables)
        {
            this.destroyables.Add(d);
        }
    }
}