using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Linq;

public class GuitarFactory : MonoBehaviour, IFactory
{
    [SerializeField] GameObject prefab;
    [SerializeField] Transform parent;

    [SerializeField] FretFactory fretFactory;

    public Guitar Create(GuitarInitData data, List<Fret> frets)
    {
        var instance = Instantiate(prefab, parent);
        var guitar = instance.GetComponent<Guitar>();
        
        guitar.Init(data, frets).Forget();

        return guitar;
    }

    public IEnumerable<Guitar> Create(IEnumerable<GuitarInitData> initDataSet)
    {
        List<Guitar> guiters = new();

        foreach(var initData in initDataSet)
        {
            var canDrags = Enumerable.Repeat(false, initData.FretIndices.Count);
            var frets = fretFactory.Create(canDrags);

            guiters.Add(Create(initData, frets));
        }

        return guiters;
    }
}
