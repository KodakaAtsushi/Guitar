using R3;
using UnityEngine;

public class SplitCountChanger : MonoBehaviour
{
    int[] splitCounts = new int[]{1, 2, 3, 4, 5, 9, 15};
    int currentIndex;
    int CurrentIndex
    {
        get { return currentIndex; }
        set
        {
            currentIndex = value;
            onCountChange.OnNext(splitCounts[currentIndex]);
        }
    }

    // events
    public Observable<int> OnCountChange => onCountChange;
    Subject<int> onCountChange = new();

    public void Init(int splitCount)
    {
        SetSplitCount(splitCount);
    }

    public void SetSplitCount(int splitCount)
    {
        CurrentIndex = GetIndex(splitCount);
    }

    public void IncrementIndex()
    {
        CurrentIndex = (currentIndex + 1) % splitCounts.Length;
    }

    int GetIndex(int splitCount)
    {
        int index = 0;

        foreach(var sp in splitCounts)
        {
            if(sp == splitCount) return index;

            index++;
        }

        throw new System.Exception($"splitCount : {splitCount}, is not found");
    }
}
