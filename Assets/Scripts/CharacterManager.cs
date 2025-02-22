using Assets.StatSystem;
using UnityEngine;


public class CharacterManager : MonoBehaviour
{
    static CharacterManager _instance;
    public static CharacterManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new();
                go.name = nameof(CharacterManager);
                go.AddComponent<CharacterManager>();
                return _instance;
            }
            else
            {
                return _instance;
            }

        }
    }
    private void Awake()
    {
        _instance = this;
    }
    public void OnCharacterInit(CharacterFacade character)
    {
        character.Init();
    }
}
