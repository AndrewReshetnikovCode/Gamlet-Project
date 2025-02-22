using UnityEngine;

public class LoadedFingerController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlayerManager.StCharacter.GetComponent<ShootingController>().LaunchBullet(transform.position, transform.forward);
            Destroy(gameObject);
        }
    }
}
