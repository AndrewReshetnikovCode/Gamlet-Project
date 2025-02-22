using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;
[CreateAssetMenu(menuName = "AI/State/RunOut")]

public class RunAwayState : AIState
{
    protected override State<StateT> GetStateProtected()
    {
        return new State<StateT>(null, OnLogic, OnExit);
    }
    void OnLogic(State<StateT, string> s)
    {
        Vector3 selfPos = Brain.Character.GetMainTransform().position;
        Vector3 target = selfPos + (selfPos - GetTransform().position);
        NavMeshHit hit;
        if (Brain.Character.navMeshMovement.Agent.Raycast(target, out hit))
        {
            Brain.Character.navMeshMovement.GoTo(hit.position);
        }
        else
        {
            Brain.Character.navMeshMovement.GoTo(target);
        }
    }
    void OnExit(State<StateT, string> s)
    {
        Brain.Character.navMeshMovement.GoTo(Brain.transform.position);
    }
    protected virtual Transform GetTransform()
    {
        return Brain.Context.TargetTransform;
    }
}