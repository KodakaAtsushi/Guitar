using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ObjectDeployer
{
    public static void Deploy(IEnumerable<MonoBehaviour> monoBehaviours, Vector3 space)
    {
        var count = monoBehaviours.Count();

        var firstPos = (float)(count-1) / 2 * space;

        var index = 0;
        foreach(var monoBehaviour in monoBehaviours)
        {
            monoBehaviour.transform.localPosition = firstPos - space * index;
            index++;
        }
    }
}
