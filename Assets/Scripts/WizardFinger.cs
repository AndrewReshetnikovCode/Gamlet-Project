using System.Collections;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class WizardFinger : WeaponBehaviour
{
    [SerializeField] LoadedFingerController _fingerControllerPrefab;
    [SerializeField] Vector3 _instantiateOffset;
    public override void OnUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Instantiate(_fingerControllerPrefab, Camera.main.transform.position + PlayerManager.StCharacter.transform.TransformVector(_instantiateOffset), Camera.main.transform.rotation);
        }
    }


}
