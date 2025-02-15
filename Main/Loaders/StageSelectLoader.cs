using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class StageSelectLoader : MonoBehaviour, ILoader
{
    // Containers
    DBContainer dBContainer;
    FactoryContainer factoryContainer;

    PlayerSaveDB playerSaveDB => dBContainer.Get<PlayerSaveDB>();
    StageSelectButtonFactory buttonFactory => factoryContainer.Get<StageSelectButtonFactory>();

    // Scene Objects
    [SerializeField] VolumeKnob bgmKnob;
    [SerializeField] VolumeKnob seKnob;

    [SerializeField] GameObject sceneParent;

    IEnumerable<StageSelectButton> buttons;

    public void Init(DBContainer dBContainer, FactoryContainer factoryContainer, SceneLoadRequester requester, VolumeChanger volumeChanger)
    {
        this.dBContainer = dBContainer;
        this.factoryContainer = factoryContainer;

        var isClearedList = playerSaveDB.GetIsClearedList();
        
        // Init Scene Objects
        buttons = buttonFactory.CreateAndDeploy(isClearedList);
        bgmKnob.Init();
        seKnob.Init();

        // Init Controllers
        new StageSelectButtonController(buttons, requester);
        new VolumeKnobController(bgmKnob, volumeChanger);
        new VolumeKnobController(seKnob, volumeChanger);
    }

    async UniTask ILoader.Load(object option)
    {
        sceneParent.SetActive(true);

        var isClearedList = playerSaveDB.GetIsClearedList();

        SetMaterials(isClearedList); // クリア済みのステージのみmaterialを更新

        await UniTask.Delay(0);
    }

    async UniTask ILoader.Unload()
    {
        sceneParent.SetActive(false);

        await UniTask.Delay(0);
    }

    void SetMaterials(IEnumerable<bool> isClearedList)
    {
        for(int i = 0; i < isClearedList.Count(); i++)
        {
            var isCleared = isClearedList.ElementAt(i);
            
            if(isCleared)
            {
                buttons.ElementAt(i).SetIsClearedMat();
            }
        }
    }
}
