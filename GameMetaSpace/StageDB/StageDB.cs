using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "StageDB", menuName = "ScriptableObject/StageDB")]
public class StageDB : ScriptableObject, IDB
{
    [SerializeField] List<Stage> stages;
    public List<Stage> Stages => stages;

    public int StageCount => stages.Count;
}

[Serializable]
public class Stage
{
    [SerializeField] List<string> noteNamesList_string;
    [SerializeField] int rollLimit = 3;
    [SerializeField] List<Vector3> playerPositions;
    [SerializeField] int fretCount = 1;
    [SerializeField] List<GuitarInitData> guitars;

    public IEnumerable<IEnumerable<NoteName>> NoteNamesList => ToNoteNamesList();
    public int RollLimit => rollLimit;
    public List<Vector3> PlayerPositions => playerPositions;
    public int FretCount => fretCount;
    public List<GuitarInitData> Guitars => guitars;

    // 例えば"C, G" を NoteName.C NoteName.Gのリストとして返す
    IEnumerable<IEnumerable<NoteName>> ToNoteNamesList()
    {
        var splitedList = noteNamesList_string.Select(noteNames_string => noteNames_string.Split(", "));

        var noteNamesList = splitedList.Select(splited => splited.Select(name_string => (NoteName)Enum.Parse(typeof(NoteName), name_string)));

        return noteNamesList;
    }
}