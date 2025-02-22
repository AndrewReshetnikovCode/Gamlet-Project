using UnityEngine;


public class Decision : ScriptableObject
{
    public AIState state;
    public virtual float Evaluate(CharacterFacade character) { return 0; }
    

}
