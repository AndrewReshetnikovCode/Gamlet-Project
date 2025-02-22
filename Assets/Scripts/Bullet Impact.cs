using UnityEngine;

public class BulletImpact : MonoBehaviour
{
    public GameObject bulletDecalPrefab; // ������ ����� �� ����
    public float decalLifetime = 10f;    // ����� ����� �����
    public new GameObject particleSystemPrefab;

    public void SpawnBulletDecal(RaycastHit hit)
    {
        // ������� ���� �� ����
        GameObject decal = Instantiate(bulletDecalPrefab, hit.point, Quaternion.LookRotation(hit.normal));

        // ������������ decal ���, ����� �� ��� �� �����������
        decal.transform.rotation = Quaternion.LookRotation(hit.normal);
        decal.transform.position += decal.transform.forward * 0.0001f; 
        // ������������ decal � ����������� (����� ���� �������� ������ � ��������)
        decal.transform.SetParent(hit.collider.transform);

        // ������� decal ����� ��������� �����
        Destroy(decal, decalLifetime);

        if (particleSystemPrefab)
        {
        Vector3 hitPoint = hit.point;
        Vector3 hitNormal = hit.normal;
        var particles = Instantiate(particleSystemPrefab, hitPoint, Quaternion.LookRotation(hitNormal));

        Destroy(particles, 1f);
        }

    }

}
