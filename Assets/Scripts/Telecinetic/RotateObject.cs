using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float speed = 1;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 360*Time.deltaTime * speed, 0, Space.World);
    }
}
