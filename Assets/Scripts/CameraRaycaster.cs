using System;
using UnityEngine;
using UnityEngine.UI;

public interface ICameraRayReceiver
{
    void OnRayEnter();
    void OnRay();
    void OnRayExit();
    float GetMaxDistance();
}

public class CameraRaycaster : MonoBehaviour
{
    public GameObject currentHitted => hit.transform?hit.transform.gameObject:null;

    [SerializeField] private LayerMask raycastLayerMask;
    [SerializeField] private float defaultMaxDistance = 10f;

    private ICameraRayReceiver lastReceiver;
    private RaycastHit hit;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);   

        if (Physics.Raycast(ray, out hit, defaultMaxDistance, raycastLayerMask))
        {
            ICameraRayReceiver receiver = hit.collider.GetComponent<ICameraRayReceiver>();

            if (receiver != null)
            {
                float maxDistance = receiver.GetMaxDistance();
                if (hit.distance > maxDistance)
                {
                    ExitLastReceiver();
                    return;
                }
                  
                if (receiver != lastReceiver)
                {
                    ExitLastReceiver();
                    receiver.OnRayEnter();
                }

                receiver.OnRay();
                lastReceiver = receiver;
            }
            else
            {
                ExitLastReceiver();
            }
        }
        else
        {
            ExitLastReceiver();
        }
    }

    private void ExitLastReceiver()
    {
        if (lastReceiver != null)
        {
            lastReceiver.OnRayExit();
            lastReceiver = null;
        }
    }
}


