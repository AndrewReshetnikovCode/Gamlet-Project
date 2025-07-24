using UnityEngine;

public class AimFovController : MonoBehaviour
{
    public Camera playerCamera;
    public float normalFOV = 60f;
    public float aimFOV = 30f;
    public float aimSpeed = 10f;

    private bool isAiming = false;
    [SerializeField] bool _ableToAim = true;
    public void SetAbilityToAim(bool value)
    {
        _ableToAim = value;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && _ableToAim)
        {
            isAiming = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
        }

        float targetFOV = isAiming ? aimFOV : normalFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, aimSpeed * Time.deltaTime);
    }
}
