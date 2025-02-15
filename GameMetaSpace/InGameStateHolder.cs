using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameStateHolder
{
    public int StageIndex { get; private set; }

    public void SetStageIndex(int nextStageIndex)
    {
        StageIndex = nextStageIndex;
    }
}
