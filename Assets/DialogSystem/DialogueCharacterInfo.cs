using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueCharacterInfo", menuName = "Dialogue System/CharacterInfo")]
public class DialogueCharacterInfo : ScriptableObject
{
    public List<DialogueCharacterData> characters;


}
[System.Serializable]
public class DialogueCharacterData
{
    public string name;
    public Color color;
    public List<AudioClip> talkingSounds;
}