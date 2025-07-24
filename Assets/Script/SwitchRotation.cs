using UnityEngine;

public class SwitchRotation : MonoBehaviour
{
    [SerializeField] Transform _t;
    [SerializeField] Vector3 _localEulerRotationOn;
    [SerializeField] Vector3 _localEulerRotationOff;
    [SerializeField] float _rotationSpeed = 5f;

    public void TurnRotation(bool isOn)
    {
        StopAllCoroutines();
        Vector3 targetRotation = isOn ? _localEulerRotationOn : _localEulerRotationOff;
        StartCoroutine(RotateTo(targetRotation));
    }

    private System.Collections.IEnumerator RotateTo(Vector3 targetRotation)
    {
        Quaternion targetQuat = Quaternion.Euler(targetRotation);
        while (Quaternion.Angle(_t.localRotation, targetQuat) > 0.1f)
        {
            _t.localRotation = Quaternion.Lerp(_t.localRotation, targetQuat, Time.deltaTime * _rotationSpeed);
            yield return null;
        }
        _t.localRotation = targetQuat;
    }
}
