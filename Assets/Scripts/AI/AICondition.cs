using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityHFSM;

[CreateAssetMenu(menuName ="AI/Condition/Empty")]
public class AICondition : ScriptableObject
{
    protected AITransition t;
    protected Brain Brain => t.Character.brain;
    public void Init(AITransition transition)
    {
        t = transition;
        OnInit();
    }
    public virtual bool Try(Transition<StateT> s)
    {
        return true;
    }
    protected virtual void OnInit()
    {
        Debug.Log($"AICondition {name} initialized");
    }
}