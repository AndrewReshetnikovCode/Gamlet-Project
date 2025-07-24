using UnityEngine;

public class LoadedFingerController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootingController sc = PlayerManager.CharacterStatic.GetComponent<ShootingController>();
            WeaponData wh = sc.weaponTypes.Find(t => t.name == "WizardHand");
            sc.LaunchBullet(transform.position, transform.forward, wh);
            Destroy(gameObject);
        }
    }
}
