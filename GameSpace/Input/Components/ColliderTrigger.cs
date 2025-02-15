using System.Collections;
using System.Collections.Generic;
using R3;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ColliderTrigger : MonoBehaviour
{
    public Observable<Collider> OnEnter => onEnter;
    Subject<Collider> onEnter = new();

    void OnTriggerEnter(Collider collider)
    {
        onEnter.OnNext(collider);
    }
}
