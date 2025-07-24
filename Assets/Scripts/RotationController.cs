using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    
    public void ApplyRotation(Vector3 dir)
    {
        Quaternion q = Quaternion.LookRotation(dir);
        Vector3 e = q.eulerAngles;
        e.x = 0;
        e.z = 0;
        transform.eulerAngles = e;
    }
    public void ApplyRotationToPoint(Vector3 point, bool yAxisOnly)
    {
        Vector3 dir = (point - transform.position).normalized;
        if (yAxisOnly)
        {
            dir.y = 0;
        }
        Quaternion q = Quaternion.LookRotation(dir);
        Vector3 e = q.eulerAngles;
        e.x = 0;
        e.z = 0;
        transform.eulerAngles = e;
    }
}
