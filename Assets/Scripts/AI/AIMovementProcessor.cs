using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.AI;

public interface INavMeshRaycaster
{
    bool Raycast(Vector3 origin, Vector3 dir, float maxDistance, out Vector3 hit);
}

public class AIMovementProcessor
{
    public class Target
    {
        public Vector3 pos;
        public Vector3 lookDir;
    }
    AIMovementSettings _s;
    Target _t;
    Vector3 _a;
    Vector3 _rotatedDir;
    INavMeshRaycaster _navMeshRaycaster;
    public AIMovementProcessor(AIMovementSettings settings, INavMeshRaycaster raycaster)
    {
        _s = settings;
        _navMeshRaycaster = raycaster;
    }
    public void UpdateTarget(Target target)
    {
        _t = target;
    }
    public void UpdateActor(Vector3 actor)
    {
        _a = actor;
    }
    public void UpdateRandomDir()
    {
        _rotatedDir = RotateRandom(_t.lookDir);
    }
    public Vector3 Chase()
    {
        if (Vector3.Distance(_a,_t.pos) < _s.stoppingDistance)
        {
            return _a;
        }
        Vector3 point = _t.pos;
        if (_s.considerTargetTransformRotation)
        {
            point += _rotatedDir * _s.stoppingDistance;
        }
        else
        {
            Vector3 dirToTarget = (_a - _t.pos).normalized;

            point -= dirToTarget * _s.stoppingDistance;
        }

        return point;
    }
    public Vector3 Move()
    {
        if (Vector3.Distance(_a, _t.pos) < 0.01f)
        {
            return _a;
        }
        return _t.pos;
    }
    public Vector3 RunAway()
    {
        Vector3 dir = (_a - _t.pos).normalized;
        Vector3 target = _a + dir * _s.fleeDistance;
        Vector3 hit;
        if (_navMeshRaycaster.Raycast(_a, dir, _s.fleeDistance, out hit))
        {
            return hit;
        }
        else
        {
            return target;
        }
    }
    Vector3 RotateRandom(Vector3 dir)
    {
        // Случайное значение угла отклонения в указанном диапазоне
        float randomAngle = Random.Range(_s.minRotation, _s.maxRotation);

        // Отклонение вправо или влево
        float direction = Random.Range(0, 2) == 0 ? -1f : 1f;
        randomAngle *= direction;

        // Применение вращения по оси Y к текущему forward
        Quaternion rotation = Quaternion.Euler(0, randomAngle, 0);
        return rotation * _t.lookDir;
    }
}
