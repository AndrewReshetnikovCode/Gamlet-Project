using UnityEngine;
using UnityHFSM;
[CreateAssetMenu(menuName = "AI/State/Default")]
public class AIState : ScriptableObject
{
    public StateT stateName;
    public Brain Brain { get; set; }
    public State<StateT> GetState()
    {
        State<StateT> s = GetStateProtected();
        s.name = stateName;
        return s;
    }
    protected virtual State<StateT> GetStateProtected()
    {
        return new State<StateT>(onLogic: (s) => {; }) { name = StateT.idle };
    }
}
