using UnityEngine;


public class Decision : ScriptableObject
{
    public AIStateSettings state;
    public virtual float Evaluate(CharacterFacade character) { return 0; }
    

}
