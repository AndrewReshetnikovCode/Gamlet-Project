using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

[CreateAssetMenu(menuName = "AI/Condition/CheckDistance")]

public class CheckDistanceToTransform : AICondition
{
    [SerializeField] LayerMask _wallLayer;
    public CheckDistanceType type;
    public enum CheckDistanceType
    {
        lessThan,
        greaterThan
    }
    public float distance;
    protected override void OnInit()
    {
        if (_wallLayer == LayerMask.GetMask("Nothing"))
        {
            _wallLayer = LayerMask.GetMask("Default");
        }
    }
    public override bool Try(Transition<StateT> t)
    {
        Transform targetTransform = GetTargetTransform();
        Vector3 targetPos = targetTransform.position + targetTransform.TransformDirection(Brain.Context.Offset);
        Vector3 selfPos = Brain.Character.GetMainTransform().position;
        if (type == CheckDistanceType.lessThan)
        {
            return Vector3.Distance(targetPos, selfPos) < distance;
        }
        else
        {
            return Vector3.Distance(targetPos, selfPos) > distance;
        }
    }
    bool RayToTarget(Vector3 targetPos)
    {
        Vector3 selfPos = Brain.Character.GetMainTransform().position;
        return Physics.Raycast(Brain.transform.position, targetPos - selfPos, distance, _wallLayer);
    }
    protected virtual Transform GetTargetTransform()
    {
        return Brain.Context.TargetTransform;
    }
}
