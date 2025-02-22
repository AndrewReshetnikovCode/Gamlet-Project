using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextController))]
public class DialogueManager : MonoBehaviour
{
    [SerializeField] public DialogueCharacterInfo characterList;
    [SerializeField] private GameObject dialogueButton;
    [SerializeField] public GameObject dialogueRootObject;
    [SerializeField] public GameObject worldCharacterNameRootObject;
    [HideInInspector] public DialogueCharacterData currentCharacter;

    [HideInInspector] public DialogueData currentDialogueData;
    [HideInInspector] public DialogueBranch currentBranch;
    [HideInInspector] public int PhraseIndex = 0;
    [HideInInspector] public bool isDialogueActive = false;
    private int currentBranchIndex = 0;

    [HideInInspector] public GameObject choise1;
    [HideInInspector] public GameObject choise2;

    private TextController textController;
    private TextController worldCharacterNameTextController;



    private void Start()
    {
        textController = GetComponent<TextController>();
        worldCharacterNameTextController = worldCharacterNameRootObject.GetComponent<TextController>();
    }

    [ContextMenu("TestStartDialogue")]
    public void TestStartDialogue()
    {
        StartDialogue(0);
    }
    public void StartDialogue(int dialogueDataId)
    {
        isDialogueActive = true;
        PhraseIndex = 0;
        currentBranchIndex = 1;
        currentDialogueData = DialogueDataList.Instance.dialogueDataList[dialogueDataId];
        currentBranch = currentDialogueData.branches[PhraseIndex];


        currentCharacter = characterList.characters.Find(n => n.name == currentBranch.characterName);
        if (currentCharacter != null)
        {
            textController.TypeWorldText(currentBranch.phrase[0].text);
            worldCharacterNameTextController.color = currentCharacter.color;
            worldCharacterNameTextController.TypeWorldText(currentCharacter.name);
            PhraseIndex++;
        }
        else Debug.Log("Что-то не то с именем персонажа.");

    }

    public void NextDialogue()
    {
        if (PhraseIndex < currentBranch.phrase.Count)
        {
            UpdateDialogue();
            PhraseIndex++;
        }

        else
        {
            switch (currentBranch.actionType)
            {
                case DialogueActionType.NextBranch:
                    PhraseIndex = 0;
                    currentBranch = currentDialogueData.branches[currentBranchIndex++];
                    CheckQuestAfterBranchEnd(currentBranch);
                    currentBranch.method?.Invoke();
                    RefreshCharacterName();
                    UpdateDialogue();
                    PhraseIndex++;
                    break;

                case DialogueActionType.Choice:
                    Vector3 spawnPosition1 = new Vector3(0, -87, 0);
                    Vector3 spawnPosition2 = new Vector3(377, -87, 0);
                    choise1 = Instantiate(dialogueButton, spawnPosition1, Quaternion.identity);
                    choise2 = Instantiate(dialogueButton, spawnPosition2, Quaternion.identity);
                    choise1.GetComponentInChildren<DialogueButtonController>().numberOfChoiseButton = currentBranch.choise1BranchIndex;
                    choise2.GetComponentInChildren<DialogueButtonController>().numberOfChoiseButton = currentBranch.choise2BranchIndex;
                    choise1.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = currentBranch.choise1Text;
                    choise2.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = currentBranch.choise2Text;
                    currentBranch.method?.Invoke();
                    isDialogueActive = false;

                    break;
                case DialogueActionType.EndDialogue:
                    CheckQuestAfterBranchEnd(currentBranch);
                    currentBranch.method?.Invoke();
                    EndDialogue();
                    break;
                case DialogueActionType.OnlyMethod:
                    currentBranch.method?.Invoke();
                    break;
                default:
                    break;
            }



        }
    }

    public void RefreshCharacterName()
    {
        //worldCharacterNameTextController.OutText();
        Destroy(worldCharacterNameTextController.rootTextGO.gameObject);

        currentCharacter = characterList.characters.Find(n => n.name == currentBranch.characterName);
        if (currentCharacter != null)
        {
            worldCharacterNameTextController.color = currentCharacter.color;
            worldCharacterNameTextController.TypeWorldText(currentCharacter.name);
        }
        else Debug.Log("Что-то не то с именем персонажа");
    }

    public void Choise(int branch)
    {
        PhraseIndex = 0;
        currentBranch = currentDialogueData.branches[branch];
        CheckQuestAfterBranchEnd(currentBranch);
        RefreshCharacterName();
        UpdateDialogue();
        PhraseIndex++;
    }

    //TODO
    private void CheckQuestAfterBranchEnd(DialogueBranch branch)
    {
        if (!string.IsNullOrEmpty(branch.questName))
        {
            int i = GameObject.Find("Quests List").GetComponent<QuestsList>().quests.FindIndex(quest => quest.name == currentBranch.questName);
            GameObject.Find("Quests List").GetComponent<QuestsList>().quests[i].isTaken = true;
        }
    }


    public void UpdateDialogue()
    {
        textController._isTextSpeedUp = false;
        textController.TypeWorldText(currentBranch.phrase[PhraseIndex].text);

    }



    //Убрать потом или перенести
    public GameObject FindGameObjectByName(string name)
    {
        // Получаем все объекты на сцене, включая неактивные
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true);

        // Проходим по всем объектам и ищем нужный
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Equals(name))
            {
                return obj; // Возвращаем найденный объект
            }
        }

        Debug.LogWarning("GameObject with name \"" + name + "\" not found.");
        return null; // Возвращаем null, если не нашли объект
    }

    private void EndDialogue()
    {
        //dialogueActivator.isDialogueStarted = false;
        isDialogueActive = false;


        //Cursor.lockState = CursorLockMode.Locked;

        Debug.Log("Dialogue ended.");


    }

    void Update()
    {
        if (isDialogueActive && Input.GetMouseButtonDown(0))
        {
            if (textController._isTextTyping == false)
            {
                if (textController._isOutTextWorking == true)
                {
                    textController._isOutTextWorking = false; //3
                }
                else
                {
                    textController.OutText(); //2
                }
            }
            else
            {
                textController._isTextSpeedUp = true; //1
            }
        }

    }

    public static DialogueManager Instance; // Ссылка на синглтон
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
