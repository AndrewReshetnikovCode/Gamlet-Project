using UnityEngine;
using UnityEngine.Events;

public class SwitchController : MonoBehaviour
{
    [SerializeField] SwitchRotation _switchRotation;
    [SerializeField] bool _isOn;

    public UnityEvent<bool> OnSwitched;

    public void Switch()
    {
        _isOn = !_isOn;
        _switchRotation.TurnRotation(_isOn);
        OnSwitched?.Invoke(_isOn);
    }
}
