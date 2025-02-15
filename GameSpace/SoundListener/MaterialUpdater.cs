using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialUpdater : MonoBehaviour
{
    MeshRenderer meshRenderer;

    public void Init()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetMaterial(Material mat)
    {
        meshRenderer.material = mat;
    }
}
