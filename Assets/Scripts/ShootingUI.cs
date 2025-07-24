using TMPro;
using UnityEngine;


public class ShootingUI : MonoBehaviour
{
    [SerializeField] ShootingController _controller;
    [SerializeField] TMP_Text _ammoCount;
    [SerializeField] HitMark _hitMark;
    private void Start()
    {
        if (_controller == null)
        {
            _controller = GameObject.FindObjectOfType<ShootingController>();
        }
        _controller.onHit += OnHit;
    }
    void Update()
    {
        string stockAmmoText = _controller.CurrentWeaponState.stockAmmo == -1 ? "∞" : _controller.CurrentWeaponState.stockAmmo.ToString();
        _ammoCount.text = _controller.CurrentWeaponState.loadedAmmo.ToString() + "/" + stockAmmoText;
    }
    void OnHit(float healthPercent)
    {
        if (healthPercent > 0.6f)
            _hitMark.currentMarks = 1;
        else if (healthPercent > 0f)
            _hitMark.currentMarks = 2;
        else
            _hitMark.currentMarks = 3;
        _hitMark.Display();


    }
}
