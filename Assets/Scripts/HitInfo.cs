using UnityEngine;

public class HitInfo
{
    public float speed;
    public float damage;
    public CharacterFacade source;
    public CharacterFacade reciever;
    public WeaponData weapon;
    public Transform hittedPart;
    public Vector3 hittedPoint => raycastHit.point;
    public Vector3 dir;
    public RaycastHit raycastHit;
}

