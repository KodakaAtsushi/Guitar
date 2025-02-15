using System.Collections;
using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEngine;

public class FretPool : MonoBehaviour
{
    [SerializeField] Vector2Int poolMatrix = new Vector2Int(3, 3);
    [SerializeField] float space = 0.8f;

    [SerializeField] GameObject fretPrefab;

    Fret[,] frets;
    
    public void Init(List<Fret> frets)
    {
        DestroyAll();
        this.frets = new Fret[poolMatrix.x, poolMatrix.y];

        foreach(var fret in frets)
        {
            PoolFret(fret);
        }
    }

    public void PoolFret(Fret fret)
    {
        // field更新
        var emptyIndex = GetFirstEmptyIndex();

        frets[emptyIndex.x, emptyIndex.y] = fret;

        // fretのtransform更新
        fret.transform.SetParent(transform);
        
        var deployPosX = space*emptyIndex.x;
        var deployPosZ = -space*emptyIndex.y;

        fret.transform.localPosition = new Vector3(deployPosX, 0, deployPosZ);

        // pick時のremove購読
        CompositeDisposable removeSubscribe = new();
        fret.OnPick.Subscribe(_ => {
            PickFret(fret);
            removeSubscribe.Dispose();
        }).AddTo(removeSubscribe);
    }

    void PickFret(Fret fret)
    {
        var xCount = poolMatrix.x;
        var yCount = poolMatrix.y;

        for(int y = 0; y < yCount; y++)
        {
            for(int x = 0; x < xCount; x++)
            {
                if(frets[x, y] == fret)
                {
                    frets[x, y] = null;
                    return;
                }
            }
        }

        throw new System.Exception("fretが見つからないのでremoveできない");
    }

    void DestroyAll()
    {
        var childCount = transform.childCount;

        for(int i = 0; i < childCount; i++)
        {
            var child = transform.GetChild(i);
            Destroy(child.gameObject);
        }
    }

    Vector2Int GetFirstEmptyIndex()
    {
        var xCount = poolMatrix.x;
        var yCount = poolMatrix.y;

        for(int y = 0; y < yCount; y++)
        {
            for(int x = 0; x < xCount; x++)
            {
                if(frets[x, y] == null) return new Vector2Int(x, y);
            }
        }

        throw new System.Exception("fret poolの空きがない");
    }
}
