using DemiurgEngine.StatSystem;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;
[CreateAssetMenu(menuName = "AI/State/Chase")]

public class ChaseState : AIState
{
    public float stoppingDistance;
    protected override State<StateT> GetStateProtected()
    {
        return new State<StateT>(OnEnter, OnLogic, OnExit);
    }
    protected virtual void OnEnter(State<StateT, string> s)
    {

    }
    protected virtual void OnLogic(State<StateT, string> s)
    {
        UpdateOffset();
        Transform t = GetTransform();
        Vector3 target = t.position + t.TransformDirection(Brain.Context.Offset);
        if (Vector3.Distance(target, Brain.Character.GetMainTransform().position) < stoppingDistance)
        { 
            Brain.Character.navMeshMovement.GoTo(Brain.Character.GetMainTransform().position);
        return; 
        }
        Vector3 sourcePos = t.position + t.TransformDirection(Brain.Context.Offset);
        NavMesh.SamplePosition(sourcePos, out NavMeshHit hit, 2, LayerMask.GetMask("Default"));      
        
        Brain.Character.navMeshMovement.GoTo(hit.position);
    }
    protected virtual void OnExit(State<StateT, string> s)
    {
        Brain.Character.navMeshMovement.GoTo(Brain.transform.position);
    }
    protected virtual Transform GetTransform()
    {
        return Brain.Context.TargetTransform;
    }
    protected virtual void UpdateOffset() {; }
}
