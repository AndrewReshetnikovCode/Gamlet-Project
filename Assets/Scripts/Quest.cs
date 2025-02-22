using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Quest : MonoBehaviour
{
    [SerializeField] public string questName;
    [SerializeField] public bool isCompleted;
    [SerializeField] public bool isTaken;
    [SerializeField] public bool isFailed;

    

    public void TakeQuest()
    {
        isTaken = true;
        Debug.Log($"{questName} quest taken.");
    }

    public void CompleteQuest()
    {
        if (isTaken && !isFailed)
        {
            isCompleted = true;
            Debug.Log($"{questName} quest completed!");
        }
        else
        {
            Debug.LogWarning($"{questName} quest cannot be completed.");
        }
    }

    public void FailQuest()
    {
        if (isTaken && !isCompleted)
        {
            isFailed = true;
            Debug.Log($"{questName} quest failed.");
        }
    }

    // Методы для получения данных диалога
    //public List<DialogueManager.DialogueBranch> GetDialogueBranches() => dialogueBranches;
}
