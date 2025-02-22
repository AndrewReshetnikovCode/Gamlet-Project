using System.Collections.Generic;
using UnityEngine;


public class TextManager : MonoBehaviour
{
    [SerializeField] public Sprite[] letterSprites;
    [SerializeField] public GameObject letterPrefab;
    public Dictionary<string, Sprite> letterDictionary = new Dictionary<string, Sprite>();
    private void Start()
    {
        foreach (Sprite sprite in letterSprites)
        {
            if (!letterDictionary.ContainsKey(sprite.name))
            {
                letterDictionary.Add(sprite.name, sprite);
            }
        }
    }
    public static TextManager Instance;
    private void Awake()
    {
        Instance = this;
    }
}
