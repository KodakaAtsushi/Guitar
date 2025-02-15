using R3;
using UnityEngine;
using System;
using System.Collections.Generic;

public class FrequencyCalculator : MonoBehaviour
{
    IReadOnlyList<IStringAnchor> anchors;
    int splitCount => anchors.Count-1;
    float width;

    CompositeDisposable disposables = new();

    public void Init(IReadOnlyList<IStringAnchor> anchors, float width)
    {
        this.anchors = anchors;
        this.width = width;
    }

    public float GetPitch(float pickedPos)
    {
        var normalizedPos = (pickedPos + width/2) / width; // pickedPosの最小値を0にしてからwidthで割る

        int rightIndex = GetNearestIndex_RightSide(normalizedPos);
        int leftIndex = GetNearestIndex_LeftSide(normalizedPos);

        return splitCount / (float)(rightIndex - leftIndex); // 弦長比の逆数がPitch
    }

    public void SetAnchors(IStringAnchor[] anchors)
    {
        this.anchors = anchors;
    }

    int GetNearestIndex_LeftSide(float normalizedPos)
    {
        // 一番近い左側のAnchorを取得
        for(int i = Mathf.FloorToInt(normalizedPos*splitCount); i > -1; i--)
        {
            var anchor = anchors[i];

            if(anchor == null) continue;

            return i; // left index
        }

        throw new Exception("左端のAnchorがない");
    }

    int GetNearestIndex_RightSide(float normalizedPos)
    {
        // 一番近い右側のAnchorを取得
        for(int i = Mathf.CeilToInt(normalizedPos*splitCount); i < anchors.Count; i++)
        {
            var anchor = anchors[i];

            if(anchor == null) continue;

            return i; // right index;
        }

        throw new Exception("右端のAnchorがない");
    }

    void OnDestory()
    {
        disposables.Dispose();
    }
}
