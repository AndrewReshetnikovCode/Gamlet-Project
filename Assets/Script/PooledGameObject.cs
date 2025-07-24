using UnityEngine;

public class PooledGameObject : MonoBehaviour
{
    public CharacterFacade character;
    public void ResetPooled()
    {
        if (character)
        {
            character.ResetValues();
        }
    }
}