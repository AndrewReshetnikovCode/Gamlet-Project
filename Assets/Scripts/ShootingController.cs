using DemiurgEngine.StatSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShootingController : MonoBehaviour
{
    public event Action onShoot;
    /// <summary>
    /// убил ли выстрел цель
    /// </summary>
    public event Action<float> onHit;

    [SerializeField] BulletTracer _bulletTracer;
    [SerializeField] BulletImpact _bulletImpact;

    [Header("ОСНОВНЫЕ НАСТРОЙКИ")]
    public List<WeaponData> weaponTypes;
    public List<WeaponState> weaponStates;
    public int selectedWeaponNum = 0;
    private AudioSource audioSource;
    public LayerMask hitMask;

    private WeaponData _currentWeapon => weaponTypes[selectedWeaponNum];
    private float nextFireTime = 0f;
    private bool isReloading = false;

    [Header("НАСТРОЙКИ СЛЕДА ОТ ПУЛИ")]
    // Параметры следа от пули
    public float maxRailDistance = 50f;
    [SerializeField] GameObject _explosionPrefab;

    List<ProjectileHandler> _handlers = new();
    [AutoAssignStat] Stat _damage;
    public int RicochetQuantity { get; set; }
    public int PierceQuantity { get; set; }
    public WeaponState CurrentWeaponState => weaponStates[selectedWeaponNum];
    int _currentPierced = 0;
    int _currentRicochets = 0;
    [Serializable]
    public class WeaponState
    {
        public int loadedAmmo = 0;
        public int stockAmmo = 0;
    }

    CharacterFacade character;

    [SerializeField] RecoilEffect _recoil;
    void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (character == null) character = GetComponent<CharacterFacade>();

        UpgradeController uc = GetComponent<UpgradeController>();
        uc.OnUpgradeAdded += ReorderHandlers;
        uc.OnUpgradeRemoved += ReorderHandlers;

        for (int i = 0; i < weaponTypes.Count; i++)
        {
            FillWeapon(i);
        }

        SelectWeapon();
    }
    void Update()
    {
        UpdateInputWeaponSelection();
        _currentWeapon.behaviuor?.OnUpdate();
        if (isReloading)
            return;
        if (Input.GetButtonDown("Fire1") && _currentWeapon.autoFire == false && Time.time >= nextFireTime)
        {
            Shoot();
        }
        if (Input.GetButton("Fire1") && _currentWeapon.autoFire == true & Time.time >= nextFireTime)
        {
            Shoot();
        }
    }
    public void FillWeapon(int num)
    {
        if (num >= weaponStates.Count)
        {
            weaponStates.Add(new());
            FillWeapon(num);
            return;
        }
        if (num >= weaponTypes.Count)
        {
            return;
        }
        weaponStates[num].loadedAmmo = weaponTypes[num].maxLoadedAmmo;
        weaponStates[num].stockAmmo = weaponTypes[num].maxStockAmmo;
    }
    public void AddHandler(ProjectileHandler h)
    {
        _handlers.Add(h);
    }
    public void RemoveHandler(ProjectileHandler h)
    {
        _handlers.Remove(h);
    }
    public void Shoot()
    {
        if (isReloading)
        {
            return;
        }
        if (CurrentWeaponState.loadedAmmo == 0)
        {
            StartCoroutine(Reload());
            return;
        }
        CurrentWeaponState.loadedAmmo--;


        _currentRicochets = 0;
        _currentPierced = 0;

        nextFireTime = Time.time + _currentWeapon.fireRate;

        audioSource.PlayOneShot(_currentWeapon.fireSound);

        if (_recoil != null)
        {
            _recoil.TriggerRecoil(_currentWeapon.recoilData);
        }

        for (int i = 0; i < _currentWeapon.bulletsPerShot; i++)
        {
            if (_currentWeapon.isRail)
            {
                Vector3 direction = Camera.main.transform.forward;
                if (_currentWeapon.bulletsPerShot > 1) // Для дробовика
                {
                    direction.x += Random.Range(-_currentWeapon.spread, _currentWeapon.spread);
                    direction.y += Random.Range(-_currentWeapon.spread, _currentWeapon.spread);
                }
                LaunchBullet(Camera.main.transform.position, direction);
            }
        }

        onShoot?.Invoke();
    }
    public void LaunchBullet(Vector3 origin, Vector3 dir)
    {
        RaycastHit[] hits = Physics.RaycastAll(origin, dir, maxRailDistance, hitMask);

        IOrderedEnumerable<RaycastHit> orderedHits;
        Vector3 bulletEndPos = origin + dir * maxRailDistance;

        if (hits.Length != 0)
        {
            orderedHits = hits.OrderBy(h => Vector3.Distance(transform.position, h.point));
        }
        else
        {
            CreateBulletTrail(origin, bulletEndPos);
            return;
        }

        foreach (var hit in orderedHits)
        {
            hit.transform.SendMessage("OnHit", 1);

            CharacterFacade hittedChar = null;
            if (LayerUtil.IsLayerInMask(hit.collider.gameObject.layer, LayerMask.GetMask("Character")))
            {
                hittedChar = hit.transform.GetComponentInParent<CharacterFacade>();
                if (hittedChar != null)
                {
                    hittedChar.ApplyDamage(_currentWeapon.damage, this);
                    if (_currentWeapon.behaviuor != null && _currentWeapon.behaviuor.effectOnProjHit != null && hittedChar.upgrade != null)
                    {
                        hittedChar.upgrade.AddUpgrade(_currentWeapon.behaviuor.effectOnProjHit);
                    }
                    CreateBulletTrail(origin, hit.point);
                    CreateExplosion(hit.point);
                    onHit?.Invoke(hittedChar.health.CurrentHealth/hittedChar.health.MaxHealth);
                }
            }
            HitInfo info = new();
            info.reciever = hittedChar;
            info.source = character;
            info.damage = _damage.BaseValue;
            info.dir = dir;
            info.raycastHit = hit;

            foreach (var item in _handlers)
            {
                item.OnHit(info);
            }

            if (hit.transform.tag == "Oil")
            {
                hit.transform.GetComponent<OilController>().FireUp();
                CreateExplosion(bulletEndPos);
                CreateBulletTrail(origin, hit.point);
                break;
            }

            //пуля попала во что то кроме персонажа
            if (hittedChar == null)
            {
                if (_currentRicochets < RicochetQuantity)
                {
                    _currentRicochets++;
                    CreateBulletTrail(origin, hit.point);
                    LaunchBullet(hit.point, Vector3.Reflect(dir.normalized, hit.normal.normalized));
                }
                else
                {
                    bulletEndPos = hit.point;
                    CreateExplosion(bulletEndPos);
                    CreateBulletTrail(origin, hit.point);
                    CreateBulletHole(hit);
                }
                break;
            }

            if (_currentPierced >= PierceQuantity)
            {
                bulletEndPos = hit.point;
                CreateExplosion(bulletEndPos);
                CreateBulletTrail(origin, hit.point);
                break;
            }
            else
            {
                _currentPierced++;
            }
        }
    }
    public void SelectWeapon()
    {
        if (_currentWeapon != null && _currentWeapon.behaviuor != null)
        {
            _currentWeapon.behaviuor.OnDeactivated();
        }
        _currentWeapon.behaviuor?.OnActivated();
    }
    void ReorderHandlers(Upgrade u)
    {
        _handlers.OrderBy((h) => h.Priority);
    }
    
    
    void CreateExplosion(Vector3 pos)
    {
        GameObject explosion = Instantiate(_explosionPrefab, pos, Quaternion.LookRotation(transform.position - pos));
        Destroy(explosion, 2f);
    }
    void CreateBulletHole(RaycastHit hitInfo)
    {
        _bulletImpact.SpawnBulletDecal(hitInfo);
    }
    void CreateBulletTrail(Vector3 startPosition, Vector3 endPosition)
    {
        if (_currentWeapon.haveTrail)
        {
            _bulletTracer.CreateTracer(startPosition, endPosition, _currentWeapon.trailPrefab);

        }
    }
    void UpdateInputWeaponSelection()
    {
        foreach (char c in Input.inputString)
        {
            if (char.IsDigit(c))
            {
                int num = c - '0';
                num--;
                if (num >= 0 && num < weaponTypes.Count)
                {
                    selectedWeaponNum = num;
                    SelectWeapon();
                }
            }
        }
    }
    IEnumerator Reload()
    {

        isReloading = true;
        yield return new WaitForSeconds(_currentWeapon.reloadTime);
        int reducedAmmo = CurrentWeaponState.stockAmmo > _currentWeapon.maxLoadedAmmo ? _currentWeapon.maxLoadedAmmo : CurrentWeaponState.stockAmmo;

        CurrentWeaponState.loadedAmmo = reducedAmmo;
        CurrentWeaponState.stockAmmo -= reducedAmmo;
        isReloading = false;
    }
}
