using System.Collections.Generic;
using UnityEngine;


public class DialogueDataList : MonoBehaviour
{

    public static DialogueDataList Instance;
    public List<DialogueData> dialogueDataList = new List<DialogueData>();
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

