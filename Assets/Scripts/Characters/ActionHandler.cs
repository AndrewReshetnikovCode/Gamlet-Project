using UnityEngine;
using System.Collections;
using Characters.Actions;
using JetBrains.Annotations;
using UnityEngine.AI;

public class ActionHandler : IActionHandler
{
    NavMeshAgent _navMeshAgent;
    Transform _t;
    public ActionHandler(Transform t, NavMeshAgent nmAgent)
    {
        _t = t;
        _navMeshAgent = nmAgent;
    }
    public void Handle(Action action)
    {
        switch (action.type)
        {
            case Action.Type.MoveOnGround:
                _navMeshAgent.SetDestination(action.targetPos);
                break;
            case Action.Type.MoveOnAir:
                _navMeshAgent.SetDestination(action.targetPos);
                break;
            case Action.Type.AttackMelee:

                break;
            case Action.Type.AttackRange:
                break;
            default:
                break;
        }
    }
}
