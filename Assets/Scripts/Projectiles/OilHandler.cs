using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OilHandler : ProjectileHandler
{
    public GameObject oilPrefab;
    [SerializeField] float _maxDistanceToGround = 100;
    const string groundLayerName = "Default";
    const string oilLayerName = "Oil";
    public float onSamePlaceSizeMult = 1.2f;
    public override float Priority => -1000;
    public override void OnHit(HitInfo info)
    {
        if (info.reciever != null)
        {
             if (info.reciever.health.CurrentHealth == 0)
            {
                RaycastHit hit;
                if (Physics.Raycast(info.hittedPoint, Vector3.down, out hit, _maxDistanceToGround, LayerMask.GetMask(groundLayerName, oilLayerName)))
                {
                    if (LayerUtil.IsLayerInMask(hit.transform.gameObject.layer, LayerMask.GetMask(oilLayerName)))
                    {
                        hit.transform.GetComponent<OilController>().Size *= onSamePlaceSizeMult;
                    }
                    else
                    {
                        Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                        GameObject.Instantiate(oilPrefab, hit.point, randomRotation);
                    }
                }
            }
        }
    }
}
