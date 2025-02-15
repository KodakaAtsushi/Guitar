using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCaster : MonoBehaviour
{
    Camera mainCam;

    public void Init()
    {
        mainCam = Camera.main;
    }

    public bool RayHit<T>(out T hitObject) where T : class
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        var hits = Physics.RaycastAll(ray);

        foreach(var hit in hits)
        {
            var casted = hit.collider.GetComponent<T>();

            if(casted == null) continue;

            hitObject = casted;
            return true;
        }

        hitObject = null;
        return false;
    }

    public Vector3 GetHitGroundPos()
    {
        var ray = mainCam.ScreenPointToRay(Input.mousePosition);

        var hits = Physics.RaycastAll(ray);

        foreach(var hit in hits)
        {
            var casted = hit.collider.GetComponent<Ground>();

            if(casted == null) continue;

            return hit.point;
        }

        throw new Exception("Ground is not found");
    }
}
