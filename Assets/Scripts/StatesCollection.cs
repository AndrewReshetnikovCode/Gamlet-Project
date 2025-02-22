using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHFSM;
using static UnityEditor.VersionControl.Asset;
[RequireComponent(typeof(Brain))]
public class StatesCollection : MonoBehaviour
{
    [SerializeField] StatesList _statesAsset;
    protected Brain brain;
    private void Awake()
    {
        brain = GetComponent<Brain>();
    }
    public virtual StatesList GetStatesListInstance()
    {
        return Instantiate(_statesAsset);
    }
    public virtual StateMachine<StateT> GetStateMachine()
    {
        StatesList statesAsset = GetStatesListInstance();
        List<AIState> states = new List<AIState>(statesAsset.states.Count);
        foreach (var item in statesAsset.states)
        {
            AIState stateClone = Instantiate(item);
            stateClone.Brain = brain;
            states.Add(stateClone);
        }
        List<AITransition> transitions = new List<AITransition>(statesAsset.transitions.Count);
        foreach (var item in statesAsset.transitions)
        {
            AITransition transitionClone = Instantiate(item);
            AICondition conditionClone = Instantiate(item.aiCondition);
            transitionClone.aiCondition = conditionClone;

            conditionClone.Init(transitionClone);
            transitionClone.Init(brain);

            transitions.Add(transitionClone);
        }
        statesAsset.states = states;
        statesAsset.transitions = transitions;

        StateMachine<StateT> sm = new();

        State<StateT> s = null;
        foreach (var item in statesAsset.states)
        {
            s = item.GetState();
            sm.AddState(s.name, s);
        }
        foreach (var item in statesAsset.transitions)
        {
            Transition<StateT> t = item.GetTransition();
            sm.AddTransition(t);
        }
        sm.SetStartState(s.name);
        return sm;
    }
    
}
