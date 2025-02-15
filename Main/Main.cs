using System;
using System.Collections.Generic;
using System.Data.Common;
using R3;
using UnityEngine;
using UnityEngine.Audio;

public class Main : MonoBehaviour
{
    [Header("=== DB ===")]
    [SerializeField] StageDB stageDB;
    PlayerSaveDB playerSaveDB = new PlayerSaveDB();

    [Header("=== Game Meta Space ===")]
    [Header("Loaders")]
    [SerializeField] StageSelectLoader stageSelectLoader;
    [SerializeField] InGameLoader inGameLoader;
    [SerializeField] ThankYouLoader thankYouLoader;
    [Header("Factorys")]
    [SerializeField] PlayerFactory playerFactory;
    [SerializeField] GuitarFactory guitarFactory;
    [SerializeField] SoundListenerFactory soundListenerFactory;
    [SerializeField] FretFactory fretFactory;
    [SerializeField] StageSelectButtonFactory stageSelectButtonFactory;
    [Header("Others")]
    [SerializeField] DragAndDropper dragAndDropper;
    [SerializeField] RayCaster rayCaster;
    [SerializeField] AudioMixer audioMixer;

    [Header("=== Input ===")]
    [SerializeField] KeyboardInput keyboardInput;
    [SerializeField] MouseInput mouseInput;

    public static int firstLoadStageIndex = 3;

    CompositeDisposable disposables = new();

    void Awake()
    {
        // Init Containers
        var dBContainer = new DBContainer();
        var factoryContainer = new FactoryContainer();
        var uIContainer = new UIContainer();

        // init DBs
        dBContainer.Add(stageDB);
        dBContainer.Add(playerSaveDB);
        playerSaveDB.Init(stageDB.Stages.Count);

        // Init Game Meta Spaces
        factoryContainer.Add(playerFactory);
        factoryContainer.Add(guitarFactory);
        factoryContainer.Add(soundListenerFactory);
        factoryContainer.Add(fretFactory);
        factoryContainer.Add(stageSelectButtonFactory);

        rayCaster.Init();
        dragAndDropper.Init(mouseInput, rayCaster);
        var sceneLoadRequester = new SceneLoadRequester();
        var inGameStateHolder = new InGameStateHolder();
        var volumeChanger = new VolumeChanger(audioMixer);

        // Init UIs
        uIContainer.Add(keyboardInput);

        // Init Loaders
        inGameLoader.Init(dBContainer, factoryContainer, uIContainer, sceneLoadRequester, inGameStateHolder);
        stageSelectLoader.Init(dBContainer, factoryContainer, sceneLoadRequester, volumeChanger);
        thankYouLoader.Init(sceneLoadRequester);
        var loadController = new LoadController(dBContainer, sceneLoadRequester, stageSelectLoader, inGameLoader, thankYouLoader, inGameStateHolder);

        // Add disposables
        disposables.Add(loadController);

        // First Load
        sceneLoadRequester.RequestToLoad(SceneType.StageSelect, delayTime:0);
    }

    void OnDestroy()
    {
        disposables.Dispose();
    }
}