using System.Collections.Generic;
using UnityEngine;
using R3;

public class RollCountLight : MonoBehaviour
{
    [Header("light objects")]
    [SerializeField] GameObject lightPrefab;
    [SerializeField] Material isOnMat;
    [SerializeField] Material isOffMat;
    List<MeshRenderer> meshRenderers = new();

    [Header("deploy params")]
    [SerializeField] Vector3 deployDir = Vector3.right;
    [SerializeField] float space = 0.6f;

    public void Init(int rollLimit, PlayerRollObserver rollObserver)
    {
        // このスクリプトをアタッチしたオブジェクトを破棄するわけではないため
        // Loaderではなく、このメソッド内で破棄
        DestroyAll();

        deployDir = deployDir.normalized;

        for(int i = 0; i < rollLimit; i++)
        {
            var deployPos = transform.position + space*i*deployDir;

            var instance = Instantiate(lightPrefab, deployPos, Quaternion.identity, transform);

            meshRenderers.Add(instance.GetComponent<MeshRenderer>());
        }

        rollObserver.OnChangeRemainingCount.Subscribe(count => {
            SetMaterial(count);
        }).AddTo(this);

        SetMaterial(rollLimit);
    }

    void DestroyAll()
    {
        foreach(var mR in meshRenderers)
        {
            Destroy(mR.gameObject);
        }

        meshRenderers = new();
    }

    void SetMaterial(int onCount)
    {
        for(int i = 0; i < meshRenderers.Count; i++)
        {
            if(i < onCount)
            {
                meshRenderers[i].material = isOnMat;
            }
            else
            {
                meshRenderers[i].material = isOffMat;
            }
        }
    }
}
