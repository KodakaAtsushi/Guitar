using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FretFactory : MonoBehaviour, IFactory
{
    [SerializeField] GameObject prefab;
    [SerializeField] FretPool fretPool;

    public void Init()
    {

    }

    public Fret Create(bool canDrag = true)
    {
        var instance = Instantiate(prefab);
        var fret = instance.GetComponent<Fret>();

        fret.Init(fretPool, canDrag);

        return fret;
    }

    public List<Fret> Create(IEnumerable<bool> canDrags)
    {
        List<Fret> frets = new(canDrags.Count());

        foreach(var canDrag in canDrags)
        {
            frets.Add(Create(canDrag));
        }

        return frets;
    }
}
