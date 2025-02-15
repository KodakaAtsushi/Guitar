using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class PlayerSaveDB : IDB
{
    string saveDataKey => "savedata";

    public void Init(int stageCount)
    {
        if(!PlayerPrefs.HasKey(saveDataKey))
        {
            var data = CreateSaveData(stageCount);
            Save(data);
        }
    }

    public void SaveIsCleared(int stageIndex, bool isCleared)
    {
        var data = Load();
        data.SetIsCleared(stageIndex, isCleared);
        Save(data);
    }

    public IEnumerable<bool> GetIsClearedList()
    {
        var data = Load();
        return data.stageSaveDataSet.Select(d => d.isCleared);
    }

    // local methods ------------------------------------

    void Save(PlayerSaveData saveData)
    {
        string jsonstr = JsonUtility.ToJson(saveData);

        PlayerPrefs.SetString("savedata", jsonstr);
    }

    PlayerSaveData Load()
    {
        if (!PlayerPrefs.HasKey(saveDataKey))
        {
            throw new Exception("Save file not found, returning empty data.");
        }

        var saveData = PlayerPrefs.GetString(saveDataKey);
        return JsonUtility.FromJson<PlayerSaveData>(saveData);
    }

    PlayerSaveData CreateSaveData(int stageCount)
    {
        var stageSaveDataSet = new List<StageSaveData>();

        for(int i = 0; i < stageCount; i ++)
        {
            stageSaveDataSet.Add(new StageSaveData(i, false));
        }

        return new PlayerSaveData(stageSaveDataSet);
    }
}

[Serializable]
public class PlayerSaveData
{
    public List<StageSaveData> stageSaveDataSet;

    internal PlayerSaveData(List<StageSaveData> stageSaveDataSet)
    {
        this.stageSaveDataSet = stageSaveDataSet;
    }

    internal void SetIsCleared(int stageIndex, bool isCleared = true)
    {
        var data = stageSaveDataSet.First(data => data.stageIndex == stageIndex);

        data.isCleared = isCleared; 
    }
}

[Serializable]
public class StageSaveData
{
    public int stageIndex;
    public bool isCleared;

    internal StageSaveData(int stageIndex, bool isCleared)
    {
        this.stageIndex = stageIndex;
        this.isCleared = isCleared;
    }
}
