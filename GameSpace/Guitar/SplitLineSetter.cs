using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitLineSetter : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    [SerializeField] Transform linesParent;

    float width;

    public void Init(int splitCount, float width)
    {
        this.width = width;

        SetSplitLine(splitCount);
    }

    public void SetSplitLine(int splitCount)
    {
        DestroyAll();

        for(int i = 1; i < splitCount; i++)
        {
            var leftEndPos = -width / 2;
            var span = width / splitCount;

            var instance = Instantiate(prefab, linesParent);
            instance.transform.localPosition = Vector3.zero + new Vector3(leftEndPos + span*i, 0, 0);
        }
    }

    void DestroyAll()
    {
        var childCount = linesParent.childCount;

        for(int i = 0; i < childCount; i++)
        {
            var child = linesParent.GetChild(i);
            Destroy(child.gameObject);
        }
    }
}
