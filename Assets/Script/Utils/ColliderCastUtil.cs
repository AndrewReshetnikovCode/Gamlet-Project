using System.Collections.Generic;
using UnityEngine;

public static class ColliderCastUtil
{
    /// <summary>
    /// �������� ����������� ��� ������ ��������������� ����������.
    /// </summary>
    public static List<RaycastHit> GetOverlappingObjects(Collider collider)
    {
        if (collider == null)
        {
            Debug.LogError("Collider is null!");
            return new List<RaycastHit>();
        }

        // �������� ����� ���� (�� ������ �������� Physics � ���� �������)
        int layerMask = GetLayerMask(collider);

        if (collider is BoxCollider boxCollider)
            return GetBoxColliderHits(boxCollider, layerMask);
        else if (collider is SphereCollider sphereCollider)
            return GetSphereColliderHits(sphereCollider, layerMask);
        else if (collider is CapsuleCollider capsuleCollider)
            return GetCapsuleColliderHits(capsuleCollider, layerMask);
        else
        {
            Debug.LogError("Unsupported collider type!");
            return new List<RaycastHit>();
        }
    }

    /// <summary>
    /// ��������� BoxCollider � ������� Physics.BoxCastAll.
    /// </summary>
    public static List<RaycastHit> GetBoxColliderHits(BoxCollider boxCollider, int layerMask)
    {
        Vector3 halfExtents = Vector3.Scale(boxCollider.size, boxCollider.transform.lossyScale) * 0.5f;
        // ��������� BoxCast ��� �������� (0 ����� ����)
        RaycastHit[] results = Physics.BoxCastAll(boxCollider.transform.position, halfExtents, boxCollider.transform.forward, boxCollider.transform.rotation, 0, layerMask);
        return new List<RaycastHit>(results);
    }

    /// <summary>
    /// ��������� SphereCollider � ������� Physics.SphereCastAll.
    /// </summary>
    public static List<RaycastHit> GetSphereColliderHits(SphereCollider sphereCollider, int layerMask)
    {
        // ��������� ������� �������
        float scaledRadius = sphereCollider.radius * Mathf.Max(sphereCollider.transform.lossyScale.x, Mathf.Max(sphereCollider.transform.lossyScale.y, sphereCollider.transform.lossyScale.z));
        RaycastHit[] results = Physics.SphereCastAll(sphereCollider.transform.position, scaledRadius, Vector3.forward, 0, layerMask);
        return new List<RaycastHit>(results);
    }

    /// <summary>
    /// ��������� CapsuleCollider � ������� Physics.CapsuleCastAll.
    /// </summary>
    public static List<RaycastHit> GetCapsuleColliderHits(CapsuleCollider capsuleCollider, int layerMask)
    {
        Vector3 point1, point2;
        GetCapsulePoints(capsuleCollider, out point1, out point2);
        RaycastHit[] results = Physics.CapsuleCastAll(point1, point2, capsuleCollider.radius, Vector3.forward, 0, layerMask);
        return new List<RaycastHit>(results);
    }

    /// <summary>
    /// ���������� ���� ������ ����������� ���������� � ������ ������ � �����������.
    /// </summary>
    private static void GetCapsulePoints(CapsuleCollider capsule, out Vector3 point1, out Vector3 point2)
    {
        Transform t = capsule.transform;
        // �������� ������ �� ������� �������, ����� �������� �������� �� ������ � ����� �������
        float halfHeight = Mathf.Max(0, (capsule.height * 0.5f) - capsule.radius);
        Vector3 direction = Vector3.up;
        if (capsule.direction == 0)
            direction = Vector3.right;
        else if (capsule.direction == 2)
            direction = Vector3.forward;

        // ��������� ������� � ������ ������ � �������� �������
        point1 = t.position + t.rotation * (capsule.center + direction * halfHeight);
        point2 = t.position + t.rotation * (capsule.center - direction * halfHeight);
    }

    /// <summary>
    /// ������������ ����� ���� �� ������ �������� Physics � ���� �������.
    /// </summary>
    private static int GetLayerMask(Collider collider)
    {
        int physicsLayerMask = Physics.DefaultRaycastLayers;
        int objectLayerMask = 1 << collider.gameObject.layer;
        return physicsLayerMask & objectLayerMask;
    }
}
