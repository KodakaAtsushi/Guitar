using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class AnswerNoteView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tMProUGUI;

    public void Init(IEnumerable<NoteName> noteNames)
    {
        SetText(string.Join(",", noteNames.Select(name => Note.ToItalianName(name))));
    }

    public void SetText(string text)
    {
        tMProUGUI.text = text;
    }

    public void SetTextColor(Color color)
    {
        tMProUGUI.color = color;
    }
}
