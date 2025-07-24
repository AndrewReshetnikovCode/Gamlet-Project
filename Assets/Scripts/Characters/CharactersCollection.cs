using System.Collections.Generic;
using UnityEngine;


public class CharactersCollection : MonoBehaviour
{
    [SerializeField] List<CharacterFacade> _c;
    public void Init()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            CharacterFacade c = transform.GetChild(i).GetComponentInChildren<CharacterFacade>();
            if (c)
            {
                _c.Add(c);
            }
            else
            {
                Destroy(c.gameObject);
            }
        }
    }
    public void Add(CharacterFacade c)
    {
        _c.Add(c);
    }
    public void Remove(CharacterFacade c)
    {
        _c.Remove(c);
    }
}
