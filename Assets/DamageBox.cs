using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBox : MonoBehaviour
{
    public float damage;
    public float interval;
    public LayerMask layerMask;
    BoxCollider _boxCollider;
    void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
        InvokeRepeating(nameof(ApplyToAll),interval,interval);   
    }
    void ApplyToAll()
    {

        List<RaycastHit> hits = ColliderCastUtil.GetBoxColliderHits(_boxCollider, layerMask);
        foreach (var hit in hits)
        {
            var c = hit.transform.GetComponent<CharacterFacade>();
            if (c != null)
            {
                c.ApplyDamage(damage, this);
            }
        }
    }

}
