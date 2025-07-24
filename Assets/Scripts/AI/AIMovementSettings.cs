using DemiurgEngine.StatSystem;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;
[CreateAssetMenu(menuName = "AI/State/Chase")]

public class AIMovementSettings : AIStateSettings
{
    public float stoppingDistance;
    public float fleeDistance;
    public Vector3 targetOffset;
    

    //учитывать ли направление целевого трансформа, если true - будет выбран угол из minRotation и maxRotation и точка преследования будет сдвинута на этот угол
    public bool considerTargetTransformRotation;
    [Range(0f, 180f)]
    public float minRotation = 0f;
    [Range(0f, 180f)]
    public float maxRotation = 30f;

}
