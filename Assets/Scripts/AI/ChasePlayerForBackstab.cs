using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHFSM;

[CreateAssetMenu(menuName ="AI/State/ChaseBackstab")]
public class ChasePlayerForBackstab : ChasePlayerState
{
    [Tooltip("ћинимальное отклонение от изначального вектора (в градусах)")]
    [Range(0f, 180f)]
    public float minRotation = 0f;

    [Tooltip("ћаксимальное отклонение от изначального вектора (в градусах)")]
    [Range(0f, 180f)]
    public float maxRotation = 30f;

    public float distance = 1;
    /// <summary>
    /// —оздает новый вектор с отклонением от текущего forward объекта.
    /// </summary>
    /// <returns>¬ектор с отклонением</returns>
    public Vector3 RotateToRandomDegrees()
    {
        // —лучайное значение угла отклонени€ в указанном диапазоне
        float randomAngle = Random.Range(minRotation, maxRotation);

        // ќтклонение вправо или влево
        float direction = Random.Range(0, 2) == 0 ? -1f : 1f;
        randomAngle *= direction;

        // ѕрименение вращени€ по оси Y к текущему forward
        Quaternion rotation = Quaternion.Euler(0, randomAngle, 0);
        return rotation * Vector3.forward;
    }
    protected override void OnEnter(State<StateT, string> s)
    {
        Brain.Context.Offset = RotateToRandomDegrees() * distance;
    }
}
