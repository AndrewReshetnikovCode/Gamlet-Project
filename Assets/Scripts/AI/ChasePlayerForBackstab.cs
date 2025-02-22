using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHFSM;

[CreateAssetMenu(menuName ="AI/State/ChaseBackstab")]
public class ChasePlayerForBackstab : ChasePlayerState
{
    [Tooltip("����������� ���������� �� ������������ ������� (� ��������)")]
    [Range(0f, 180f)]
    public float minRotation = 0f;

    [Tooltip("������������ ���������� �� ������������ ������� (� ��������)")]
    [Range(0f, 180f)]
    public float maxRotation = 30f;

    public float distance = 1;
    /// <summary>
    /// ������� ����� ������ � ����������� �� �������� forward �������.
    /// </summary>
    /// <returns>������ � �����������</returns>
    public Vector3 RotateToRandomDegrees()
    {
        // ��������� �������� ���� ���������� � ��������� ���������
        float randomAngle = Random.Range(minRotation, maxRotation);

        // ���������� ������ ��� �����
        float direction = Random.Range(0, 2) == 0 ? -1f : 1f;
        randomAngle *= direction;

        // ���������� �������� �� ��� Y � �������� forward
        Quaternion rotation = Quaternion.Euler(0, randomAngle, 0);
        return rotation * Vector3.forward;
    }
    protected override void OnEnter(State<StateT, string> s)
    {
        Brain.Context.Offset = RotateToRandomDegrees() * distance;
    }
}
