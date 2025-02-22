using System;
using UnityEngine;
using UnityEngine.Events;
using UnityHFSM;
[CreateAssetMenu(menuName ="AI/Transition")]
public class AITransition : ScriptableObject
{
    public AICondition aiCondition;

    public event Action onTransition;
    public CharacterFacade Character { get; private set; }
    Brain _brain;

    public Transition<StateT> transition;
    public string sendMessageMethodName;
    public void Init(Brain brain)
    {
        _brain = brain;
        Character = brain.Character;
        transition = new Transition<StateT>(transition.from, transition.to, aiCondition.Try, OnTransition);
    }
    void OnTransition(Transition<StateT> t)
    {
        if (sendMessageMethodName != string.Empty)
        {
            Character.SendMessage(sendMessageMethodName);
        }
        onTransition?.Invoke();
    }
    public Transition<StateT> GetTransition()
    {
        return transition;
    }
}
