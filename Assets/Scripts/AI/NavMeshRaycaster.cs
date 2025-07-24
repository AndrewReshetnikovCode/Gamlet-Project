using UnityEngine;
using UnityEngine.AI;

public class NavMeshRaycaster : MonoBehaviour, INavMeshRaycaster
{
    NavMeshAgent _a;
    AIMovementSettings _s;
    public NavMeshRaycaster(NavMeshAgent navMeshAgent, AIMovementSettings settings)
    {
        _a = navMeshAgent;
        _s = settings;
    }
    public virtual bool Raycast(Vector3 origin, Vector3 dir, float maxDistance, out Vector3 hit)
    {
        NavMeshHit raycastHit;
        bool result = _a.Raycast(origin + dir * maxDistance, out raycastHit);
        hit = raycastHit.position;
        return result;
    }
}