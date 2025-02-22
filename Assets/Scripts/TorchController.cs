using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchController : MonoBehaviour
{
    public void OnHit(object info)
    {

        gameObject.GetComponentInChildren<Light>().enabled = true;
    }
}
