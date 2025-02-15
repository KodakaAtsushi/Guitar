using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Vector3 offset = new Vector3(-1, 0, 0);

    public void MoveTo(Vector3 pos)
    {
        transform.position = pos + offset;
    }
}
