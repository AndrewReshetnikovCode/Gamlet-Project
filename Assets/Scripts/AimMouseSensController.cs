using UnityEngine;

public class AimMouseSensController : MonoBehaviour
{
    [SerializeField] float mouseSensMultOnAim;
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            GetComponent<PlayerCamera>().mouseSensitivity *= mouseSensMultOnAim;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            GetComponent<PlayerCamera>().mouseSensitivity /= mouseSensMultOnAim;
        }
    }
}
