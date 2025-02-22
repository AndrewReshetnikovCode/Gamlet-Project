using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchActiveOnKey : MonoBehaviour
{
    public KeyCode key;
    public KeyCode enableKey;
    public GameObject switchedGameObject;
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            switchedGameObject.SetActive(false);
        }
        if (Input.GetKeyDown(enableKey) && switchedGameObject.activeSelf == false)
        {
            switchedGameObject.SetActive(true);    
        }
    }

}
