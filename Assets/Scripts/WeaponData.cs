using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapon")]
public class WeaponData : ScriptableObject
{
    public bool ableToAim = true;
    public bool holeOnHitWall = true;
    public GameObject particlesOnHitWall;
    public string reloadAnimTrigger;
    public int modelChildNum;
    public RecoilData recoilData;
    public WeaponBehaviour behaviuor;
    public bool haveTrail;
    public ParticleSystem trailPrefab;
    public bool autoFire = false;
    public bool isRail; 
    public string weaponName;
    public float fireRate = 0.5f;       
    public int bulletsPerShot = 1;       
    public int maxLoadedAmmo = 6;             
    public int maxStockAmmo = 120;
    public float reloadTime = 2f;        
    public float damage = 10f;          
    public float spreadAngle = 0.1f;          
    public AudioClip fireSound;         
}
[Serializable]
public class RecoilData
{
    public float recoilAmountX = 2f;  // Максимальное смещение вверх по оси X
    public float recoilSpeed = 10f;   // Скорость применения отдачи
    public float returnSpeed = 5f;    // Скорость возвращения камеры в исходное положение
}
