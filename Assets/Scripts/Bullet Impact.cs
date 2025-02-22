using UnityEngine;

public class BulletImpact : MonoBehaviour
{
    public GameObject bulletDecalPrefab; // Префаб следа от пули
    public float decalLifetime = 10f;    // Время жизни следа
    public new GameObject particleSystemPrefab;

    public void SpawnBulletDecal(RaycastHit hit)
    {
        // Создаем след от пули
        GameObject decal = Instantiate(bulletDecalPrefab, hit.point, Quaternion.LookRotation(hit.normal));

        // Поворачиваем decal так, чтобы он был на поверхности
        decal.transform.rotation = Quaternion.LookRotation(hit.normal);
        decal.transform.position += decal.transform.forward * 0.0001f; 
        // Присоединяем decal к поверхности (чтобы след двигался вместе с объектом)
        decal.transform.SetParent(hit.collider.transform);

        // Удаляем decal через некоторое время
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
