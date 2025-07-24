using UnityEngine;

public class RecoilEffect : MonoBehaviour
{
    public float currentSpread = 0f;

    Quaternion _originalRotation;
    Quaternion _currentRotation;
    Quaternion _targetRotation;

    Vector3 _originalPosition;
    Vector3 _currentPosition;
    Vector3 _targetPosition;

    Camera _mainCamera;
    bool _isRecoiling = false;

    [SerializeField] float maxSpread = 5f; 

    RecoilData _d;
    [SerializeField] float _spreadDecreaseRate = 1;
    void Start()
    {
        _mainCamera = Camera.main;
        if (_mainCamera != null)
        {
            _originalRotation = _mainCamera.transform.localRotation;
            _originalPosition = _mainCamera.transform.localPosition;
            _currentRotation = _originalRotation;
            _currentPosition = _originalPosition;
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
        if (_isRecoiling)
        {
            UpdateRecoil();
        }
        else
        {
            currentSpread -= _spreadDecreaseRate * Time.deltaTime * 10;
            if (currentSpread < 0)
            {
                currentSpread = 0;
            }
            ReturnToOriginalPosition();
        }
    }

    public void TriggerRecoil(RecoilData recoilData)
    {
        if (_mainCamera == null) return;
        _d = recoilData;

        //float recoilFactor = Mathf.Clamp(currentSpread, 0, maxSpread);
        float recoilX = Random.Range((_d.recoilAmountX / 2), _d.recoilAmountX);
        currentSpread = recoilX;
        Quaternion recoilRotation = Quaternion.Euler(-recoilX, 0, 0);
        _targetRotation = _currentRotation * recoilRotation;

        _isRecoiling = true;
    }

    private void UpdateRecoil()
    {
        _currentRotation = Quaternion.Slerp(_currentRotation, _targetRotation, _d.recoilSpeed * Time.deltaTime);
        _mainCamera.transform.localRotation = _currentRotation;

        _mainCamera.transform.localPosition = _currentPosition;

        float angle = Quaternion.Angle(_currentRotation, _targetRotation);
        if (angle < 0.1f && 
            Vector3.Distance(_currentPosition, _targetPosition) < 0.01f)
        {
            _isRecoiling = false;
        }
    }
    private void ReturnToOriginalPosition()
    {
        _currentRotation = Quaternion.Slerp(_currentRotation, _originalRotation, _d.returnSpeed * Time.deltaTime);
        _mainCamera.transform.localRotation = _currentRotation;

        _mainCamera.transform.localPosition = _currentPosition;
    }


    public void ResetRecoil()
    {
        currentSpread = 0f;
    }
}
