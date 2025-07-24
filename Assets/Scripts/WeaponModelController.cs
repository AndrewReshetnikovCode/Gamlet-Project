using InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponModelController : MonoBehaviour
{
    const float FIRE_FLASH_TIME = .1f;

    [SerializeField] ShootingController _sc;
    [SerializeField] ItemsCollection _runeStonesCollection;
    [SerializeField] List<GameObject> _runeStonesModels;
    [SerializeField] GameObject _flashSprite;
    [SerializeField] GameObject _flashSprite2;
    [SerializeField] bool _isDoubeBarreled;
    private void OnEnable()
    {
        _sc.onShoot += StartDisplayFlash;
    }
    private void OnDisable()
    {
        _sc.onShoot -= StartDisplayFlash;
    }
    void Update()
    {
        HandleRunes();
    }
    void HandleRunes()
    {
        if (_runeStonesCollection == null)
        {
            return;
        }
        for (int i = 0; i < _runeStonesModels.Count; i++)
        {
            bool slotEquipped = _runeStonesCollection[i].item != null;
            _runeStonesModels[i].gameObject.SetActive(slotEquipped);
        }
    }
    void StartDisplayFlash()
    {
        StartCoroutine(DisplayFlash());
    }
    IEnumerator DisplayFlash()
    {
        if (_isDoubeBarreled)
        {
            if (_sc.CurrentWeaponState.loadedAmmo == 0)
            {
                _flashSprite.SetActive(true);
            }
            else
            {
                _flashSprite2.SetActive(true);
            }
        }
        else
        {
            _flashSprite.SetActive(true);
        }
        yield return new WaitForSeconds(FIRE_FLASH_TIME);
        _flashSprite.SetActive(false);
        if (_flashSprite2 != null)
            _flashSprite2.SetActive(false);
    }
}
