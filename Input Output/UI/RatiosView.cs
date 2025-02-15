using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RatiosView : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] RectTransform areaRectTransform;

    Animator animator;

    bool isVisible = false;

    void Awake()
    {
        animator = areaRectTransform.GetComponent<Animator>();

        button.onClick.AddListener(() => ToggleVisibility());
    }

    void ToggleVisibility()
    {
        if(isVisible)
        {
            Hide();
        }
        else
        {
            Open();
        }
    }

    void Open()
    {
        animator.Play("OpenTextArea");
        isVisible = true;
    }

    void Hide()
    {
        animator.Play("HideTextArea");
        isVisible = false;
    }

    void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}
