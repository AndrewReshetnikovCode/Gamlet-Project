using UnityEngine;

public class RecoilEffect : MonoBehaviour
{
    private Quaternion originalRotation;
    private Quaternion currentRotation;
    private Quaternion targetRotation;

    private Vector3 originalPosition;
    private Vector3 currentPosition;
    private Vector3 targetPosition;

    private Camera mainCamera;
    private bool isRecoiling = false;

    public float currentSpread = 0f;

    [SerializeField] float maxSpread = 5f; 

    RecoilData _d;
    [SerializeField] float _spreadDecreaseRate = 1;
    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera != null)
        {
            originalRotation = mainCamera.transform.localRotation;
            originalPosition = mainCamera.transform.localPosition;
            currentRotation = originalRotation;
            currentPosition = originalPosition;
        }
        else
        {
            Debug.LogError("Main Camera not found!");
        }
    }

    void LateUpdate()
    {
        if (_d == null)
        {
            return;
        }
        if (isRecoiling)
        {
            UpdateRecoil();
        }
        else
        {
            currentSpread -= _spreadDecreaseRate * Time.deltaTime;
            if (currentSpread < 0)
            {
                currentSpread = 0;
            }
            ReturnToOriginalPosition();
        }
    }

    public void TriggerRecoil(RecoilData recoilData)
    {
        if (mainCamera == null) return;
        _d = recoilData;

        //float recoilFactor = Mathf.Clamp(currentSpread, 0, maxSpread);
        float recoilX = Random.Range((_d.recoilAmountX / 2), _d.recoilAmountX);
        currentSpread = recoilX;
        Quaternion recoilRotation = Quaternion.Euler(-recoilX, 0, 0);
        targetRotation = currentRotation * recoilRotation;

        isRecoiling = true;
    }

    private void UpdateRecoil()
    {
        currentRotation = Quaternion.Slerp(currentRotation, targetRotation, _d.recoilSpeed * Time.deltaTime);
        mainCamera.transform.localRotation = currentRotation;

        mainCamera.transform.localPosition = currentPosition;

        float angle = Quaternion.Angle(currentRotation, targetRotation);
        if (angle < 0.1f && 
            Vector3.Distance(currentPosition, targetPosition) < 0.01f)
        {
            isRecoiling = false;
        }
    }
    private void ReturnToOriginalPosition()
    {
        currentRotation = Quaternion.Slerp(currentRotation, originalRotation, _d.returnSpeed * Time.deltaTime);
        mainCamera.transform.localRotation = currentRotation;

        mainCamera.transform.localPosition = currentPosition;
    }


    public void ResetRecoil()
    {
        currentSpread = 0f;
    }
}
