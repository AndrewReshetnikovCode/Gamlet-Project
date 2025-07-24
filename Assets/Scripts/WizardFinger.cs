using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class WizardFinger : WeaponBehaviour
{
    [Serializable]
    public class Settings
    {
        public int maxHands;
    }
    public Settings settings;
    [SerializeField] LoadedFingerController _fingerControllerPrefab;
    [SerializeField] Vector3 _instantiateOffset;

    public override void OnUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (GameState.wizardHandsInstantiatedInWorld < settings.maxHands && ShootingController.instance.CurrentWeaponState.loadedAmmo != 0)
            {
                ShootingController.instance.CurrentWeaponState.loadedAmmo--;

                Instantiate(_fingerControllerPrefab, Camera.main.transform.position + PlayerManager.CharacterStatic.transform.TransformVector(_instantiateOffset), Camera.main.transform.rotation);
                GameState.wizardHandsInstantiatedInWorld++;
            }
        }
    }
    public override void OnShoot()
    {
        GameState.wizardHandsInstantiatedInWorld = 0;
    }
    public override void OnActivated()
    {
        UIManager.instance.WizardHandCounter.gameObject.SetActive(true);
    }
    public override void OnDeactivated()
    {
        UIManager.instance.WizardHandCounter.gameObject.SetActive(false);
    }
}
