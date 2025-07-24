using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHFSM;
[RequireComponent(typeof(Brain))]
public class SStateMachine : MonoBehaviour
{
    private void Awake()
    {
        StateMachine<StateT> holdDistance = new();
        StateMachine<StateT> rushTarget = new();
        StateMachine<StateT> swarmTarget = new();


    }
    
    //public virtual StateMachine<StateT> GetStateMachine()
    //{
    //    StatesList statesAsset = GetStatesListInstance();
    //    List<AIStateSettings> states = new List<AIStateSettings>(statesAsset.states.Count);
    //    foreach (var item in statesAsset.states)
    //    {
    //        AIStateSettings stateClone = Instantiate(item);
    //        stateClone.Brain = brain;
    //        states.Add(stateClone);
    //    }
    //    List<AITransition> transitions = new List<AITransition>(statesAsset.transitions.Count);
    //    foreach (var item in statesAsset.transitions)
    //    {
    //        AITransition transitionClone = Instantiate(item);
    //        AICondition conditionClone = Instantiate(item.aiCondition);
    //        transitionClone.aiCondition = conditionClone;

    //        conditionClone.Init(transitionClone);
    //        transitionClone.Init(brain);

    //        transitions.Add(transitionClone);
    //    }
    //    statesAsset.states = states;
    //    statesAsset.transitions = transitions;

    //    StateMachine<StateT> sm = new();

    //    State<StateT> s = null;
    //    foreach (var item in statesAsset.states)
    //    {
    //        s = item.GetState();
    //        sm.AddState(s.name, s);
    //    }
    //    foreach (var item in statesAsset.transitions)
    //    {
    //        Transition<StateT> t = item.GetTransition();
    //        sm.AddTransition(t);
    //    }
    //    sm.SetStartState(s.name);
    //    return sm;
    //}
    
}
