using System.Collections;
using System.Collections.Generic;
using R3;
using UnityEngine;

public class StageSelectButton : MonoBehaviour
{
    GameSpaceButton button;

    public Observable<int> OnClick => button.OnClick.Select(_ => stageIndex);

    int stageIndex;

    [SerializeField] Material isClearedMat;

    public void Init(int stageIndex, bool isCleared)
    {
        button = GetComponent<GameSpaceButton>();
        button.Init();
        
        this.stageIndex = stageIndex;

        if(isCleared) SetMaterial(isClearedMat);
    }

    public void SetIsClearedMat()
    {
        SetMaterial(isClearedMat);
    }

    void SetMaterial(Material mat)
    {
        var mr = GetComponent<MeshRenderer>();
        mr.material = mat;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
