using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GuitarInitData
{
    [SerializeField] Vector3 position;
    [SerializeField] int splitCount;
    [SerializeField] List<int> fretIndices;
    [SerializeField] bool isButtonEnable = false;

    public Vector3 Position => position;
    public int SplitCount => splitCount;
    public List<int> FretIndices => fretIndices;
    public bool IsButtonEnable => fretIndices.Count == 0 && isButtonEnable;
}
