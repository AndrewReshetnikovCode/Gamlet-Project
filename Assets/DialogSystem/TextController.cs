using System.Collections;
using UnityEngine;


public class TextController : MonoBehaviour
{
    [HideInInspector] public bool _isTextTyping;
    [SerializeField] private Vector3 topLeftCorner;
    [SerializeField] private Vector3 bottomRightCorner;
    [SerializeField] private float fontSize = 1f;
    [SerializeField] private float spacing = 0.5f;
    [SerializeField] private float verticalSpacing = 0.5f;
    [SerializeField] private float wordSpacing = 0.5f;
    [SerializeField] public float animationSpeed = 1f;
    [SerializeField] public bool isDialogue;
    [SerializeField] public Color color;
    [HideInInspector] public GameObject rootTextGO;
    private AudioSource audioSource;
    private bool _hasAudioSource = false;
    private DialogueManager DM;

    [HideInInspector] public bool _isTextSpeedUp = false;
    [HideInInspector] public bool _isOutTextWorking = false;


    private bool isRainbow;

    private void Start()
    {
        if (GetComponent<AudioSource>())
        {
            audioSource = GetComponent<AudioSource>();
            _hasAudioSource = true;
        }
        DM = DialogueManager.Instance;
    }

    public void TypeWorldText(string text)
    {
        StartCoroutine(TypeText(text)); 
    }   

    public IEnumerator TypeText(string text)
    {
        if (rootTextGO != null)
        {
            Destroy(rootTextGO);
        }
        if (_hasAudioSource)
        {
            if (isDialogue)
            {
                audioSource.PlayOneShot(DM.currentCharacter.talkingSounds[Random.Range(0, DialogueManager.Instance.currentCharacter.talkingSounds.Count - 1)]);
            }
            else
            {
                //TODO
            }
        }
        _isTextTyping = true;

        GameObject letterArray = new GameObject("===Text GO===");
        letterArray.transform.SetParent(transform);
        rootTextGO = letterArray;

        float lineWidth = bottomRightCorner.x - topLeftCorner.x;
        Vector3 currentPosition = topLeftCorner;
        string[] words = text.Split(' '); // Разделяем текст на слова

        int counterForWordEffects = 0;

        foreach (string word in words)
        {

            // Сначала проверяем ширину слова
            float wordWidth = 0f;
            foreach (char c in word)
            {
                wordWidth += fontSize + spacing;
            }
            // Перенос
            if (currentPosition.x + wordWidth > bottomRightCorner.x)
            {
                currentPosition.x = topLeftCorner.x; // Возвращаемся в начало строки
                currentPosition.y -= fontSize + verticalSpacing; // Сдвигаем вниз для новой строки
            }
            // Теперь добавляем каждую букву в строку
            foreach (char c in word)
            {
                if (isDialogue)
                {
                    if (c == '<')
                    {
                        color = DM.currentBranch.phrase[DM.PhraseIndex - 1].wordsColorEffects[counterForWordEffects];
                        counterForWordEffects++;
                        continue;
                    }
                    if (c == '>')
                    {
                        color = Color.white;
                        continue;
                    }
                    //Ⓜ Ⓔ
                }
                if (c == 'Ⓡ')
                {
                    if (isRainbow == true)
                    {
                        isRainbow = false;
                        color = Color.white;
                    }
                    else
                    {
                        isRainbow = true;
                        color = Color.red;
                    }
                    continue;
                }
                GameObject letterRootGO = new GameObject("===Letter===");
                letterRootGO.transform.SetParent(letterArray.transform);
                letterRootGO.transform.position = currentPosition;
                letterRootGO.transform.localScale = Vector3.one * fontSize;
                GameObject letterObj = Instantiate(TextManager.Instance.letterPrefab, letterRootGO.transform);
                letterObj.GetComponent<SpriteRenderer>().color = color;

                if (isRainbow) letterObj.AddComponent<ColorHueRotator>();

                


                if (_hasAudioSource)
                {
                    if (!audioSource.isPlaying)
                    {
                        audioSource.PlayOneShot(DM.currentCharacter.talkingSounds[Random.Range(0, DM.currentCharacter.talkingSounds.Count - 1)]);
                    }
                }


                // Устанавливаем спрайт символа
                SpriteRenderer spriteRenderer = letterObj.GetComponent<SpriteRenderer>();
                if (char.IsLower(c) && char.IsLetter(c))
                {
                    spriteRenderer.sprite = TextManager.Instance.letterDictionary[c.ToString() + "_"];
                }
                else if (c == '.')
                {
                    spriteRenderer.sprite = TextManager.Instance.letterDictionary["dot"];
                }
                else
                {
                    spriteRenderer.sprite = TextManager.Instance.letterDictionary[c.ToString()];
                }
                if (_isTextSpeedUp == false)
                {
                    yield return new WaitForSeconds(animationSpeed);
                }
                currentPosition.x += fontSize + spacing;

            }

            currentPosition.x += wordSpacing;

        }
        _isTextTyping = false;
        counterForWordEffects = 0;
    }     

    public void OutText()
    {
        StartCoroutine(OutAnimation());
    }

    private IEnumerator OutAnimation()
    {
        _isOutTextWorking = true;
        int childCount = rootTextGO.transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            if (i < rootTextGO.transform.childCount) // Проверяем, что индекс в пределах
            {
                Transform child = rootTextGO.transform.GetChild(i);
                child.transform.GetComponentInChildren<Animator>().SetTrigger("Out");
                Destroy(child.gameObject, 0.45f);
                if (_isOutTextWorking)
                {
                    yield return new WaitForSeconds(0.01f);
                }
            }
        }
        Destroy(rootTextGO);
        _isOutTextWorking = false;

        if (isDialogue)
        {
            DM.NextDialogue();
        }
    }
}