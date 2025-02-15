using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageSelectButtonFactory : MonoBehaviour, IFactory
{
    [SerializeField] GameObject prefab;
    [SerializeField] Transform parent;

    public List<StageSelectButton> CreateAndDeploy(IEnumerable<bool> isClearedList)
    {
        List<StageSelectButton> buttons = new();

        for(int i = 0; i < isClearedList.Count(); i++)
        {
            var instance = Instantiate(prefab, parent);
            var button = instance.GetComponent<StageSelectButton>();
            button.Init(i, isClearedList.ElementAt(i));

            buttons.Add(button);
        }

        SetPositions(buttons);

        return buttons;
    }


    [SerializeField] Vector2 space = Vector2.one;
    [SerializeField] int columnCount = 5;

    // parent の childcountを参照して位置調整
    void SetPositions(List<StageSelectButton> buttons)
    {
        var rowCount = Mathf.CeilToInt((float)buttons.Count/columnCount);
        var startPos = new Vector3(-space.x * (columnCount-1)/2, 0, space.y * (rowCount-1)/2);

        for(int i = 0; i < buttons.Count; i++)
        {
            var offsetX = i % columnCount * space.x;
            var offsetZ = i / columnCount * space.y;

            var button = buttons[i];
            var position = startPos + new Vector3(offsetX, 0, -offsetZ);
            button.transform.position = position;
        }
    }
}
