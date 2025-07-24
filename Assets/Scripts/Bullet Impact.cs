using UnityEngine;

public class BulletImpact : MonoBehaviour
{
    public GameObject bulletDecalPrefab; // Префаб следа от пули
    public float decalLifetime = 10f;    // Время жизни следа

    public void Execute(RaycastHit hit, bool createHole, GameObject particleSystemPrefab)
    {
        if (createHole)
        {
        GameObject decal = Instantiate(bulletDecalPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            decal.transform.rotation = Quaternion.LookRotation(hit.normal);
            decal.transform.position += decal.transform.forward * 0.0001f;

            decal.transform.SetParent(hit.collider.transform);

            Destroy(decal, decalLifetime);
        }


        if (particleSystemPrefab)
        {
        Vector3 hitPoint = hit.point;
        Vector3 hitNormal = hit.normal;
        var particles = Instantiate(particleSystemPrefab, hitPoint, Quaternion.LookRotation(hitNormal));

        Destroy(particles, 1f);
        }

    }

}
