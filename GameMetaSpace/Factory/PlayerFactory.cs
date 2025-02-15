using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFactory : MonoBehaviour, IFactory
{
    [SerializeField] GameObject prefab;
    [SerializeField] Transform parent;

    public Player Create(Vector3 localPos)
    {
        var instance = Instantiate(prefab, parent);
        instance.transform.localPosition = localPos;

        var player = instance.GetComponent<Player>();
        player.Init();

        return player;
    }

    public IEnumerable<Player> Create(IEnumerable<Vector3> localPositions)
    {
        List<Player> players = new();

        foreach(var localPos in localPositions)
        {
            players.Add(Create(localPos));
        }

        return players;
    }
}
