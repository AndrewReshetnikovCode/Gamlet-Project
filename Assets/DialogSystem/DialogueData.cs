using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum DialogueActionType
{
    NextBranch,
    Choice,
    EndDialogue,
    OnlyMethod
}


[System.Serializable]
public class DialogueBranch
{
    
    public List<DialogueTextField> phrase;
    public DialogueActionType actionType; // Действие после завершения диалога

    public string characterName;

    public string questName;

    public int choise1BranchIndex;
    public string choise1Text;

    public int choise2BranchIndex;
    public string choise2Text;

    public UnityEvent method;
    
    [System.Serializable]
    public class DialogueTextField
    {
        [TextArea]
        public string text;
        public List<Color> wordsColorEffects;
    }
}

[CreateAssetMenu(fileName = "DialogueData", menuName = "Dialogue System/DialogueData")]
public class DialogueData : ScriptableObject
{
    public List<DialogueBranch> branches; // Список всех веток диалога
}
