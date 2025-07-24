using UnityEngine;

public enum CharacterType
{
    dummy,
    gremlin,
    flyingHead
}
public class CharacterInstantiator : MonoBehaviour
{
    CharactersCollection _c;

    [SerializeField] CharacterFacade _dummyPf;
    [SerializeField] CharacterFacade _gremlinPf;
    [SerializeField] CharacterFacade _flyingHeadPf;
    public void Init(CharactersCollection c)
    {
        _c = c;
    }
    public CharacterFacade InstantiateCharacter(CharacterType type)
    {
        CharacterFacade character;
        switch (type)
        {
            case CharacterType.dummy:
                character = Instantiate(_dummyPf);
                break;
            case CharacterType.gremlin:
                character = Instantiate(_gremlinPf);
                break;
            case CharacterType.flyingHead:
                character = Instantiate(_flyingHeadPf);
                break;
            default:
                return null;
        }

        _c.Add(character);
        return character;
    }
    
}
