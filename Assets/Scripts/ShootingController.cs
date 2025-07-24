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
    public event Action onReloadStart;
    public event Action onReloadEnd;
    /// <summary>
    /// убил ли выстрел цель
    /// </summary>
    public event Action<float> onHit;

    public static ShootingController instance;

    [Serializable]
    public class WeaponState
    {
        public int loadedAmmo = 0;
        public int stockAmmo = 0;
    }

    [Header("ОСНОВНЫЕ НАСТРОЙКИ")]
    public List<WeaponData> weaponTypes;
    public List<WeaponState> weaponStates;

    public LayerMask hitMask;

    public WeaponData CurrentWeapon => weaponTypes[_selectedWeaponNum];

    public WeaponState CurrentWeaponState => weaponStates[_selectedWeaponNum];

    [Header("НАСТРОЙКИ СЛЕДА ОТ ПУЛИ")]
    public Vector3 originOffset;
    public float maxRailDistance = 50f;
    public int RicochetQuantity { get; set; }
    public int PierceQuantity { get; set; }

    [SerializeField] BulletImpact _bulletSurfaceImpact;

    [SerializeField] RecoilEffect _recoil;

    [SerializeField] Transform _modelParent;
    [SerializeField] Transform CurrentModel => _modelParent.GetChild(CurrentWeapon.modelChildNum);

    [SerializeField] int _weaponAtStart;

    int _selectedWeaponNum = 0;
    AudioSource audioSource;

    float nextFireTime = 0f;
    bool isReloading = false;

    List<ProjectileHandler> _handlers = new();
    
    int _currentPierced = 0;
    int _currentRicochets = 0;
    

    CharacterFacade _character;

    [SerializeField] AimFovController _aimController;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (_character == null) _character = GetComponent<CharacterFacade>();

        UpgradesController uc = GetComponent<UpgradesController>();
        uc.OnUpgradeAdded += ReorderHandlers;
        uc.OnUpgradeRemoved += ReorderHandlers;

        for (int i = 0; i < weaponTypes.Count; i++)
        {
            FillWeapon(i);
        }

        SelectWeapon(_weaponAtStart);
    }
    void Update()
    {
        UpdateInputWeaponSelection();
        CurrentWeapon.behaviuor?.OnUpdate();
        if (isReloading)
            return;
        if (Input.GetButtonDown("Fire1") && CurrentWeapon.autoFire == false && Time.time >= nextFireTime)
        {
            Shoot();
        }
        if (Input.GetButton("Fire1") && CurrentWeapon.autoFire == true & Time.time >= nextFireTime)
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
            if (CurrentWeapon != null && CurrentWeapon.behaviuor != null)
            {
                CurrentWeapon.behaviuor.OnShoot();
            }
            return;
        }
        CurrentWeaponState.loadedAmmo--;


        _currentRicochets = 0;
        _currentPierced = 0;

        nextFireTime = Time.time + CurrentWeapon.fireRate;

        audioSource.PlayOneShot(CurrentWeapon.fireSound);

        if (_recoil != null)
        {
            _recoil.TriggerRecoil(CurrentWeapon.recoilData);
        }

        for (int i = 0; i < CurrentWeapon.bulletsPerShot; i++)
        {
            if (CurrentWeapon.isRail)
            {
                Vector3 direction = Camera.main.transform.forward;
                if (i != 0)
                {
                    Quaternion rotation = Quaternion.AngleAxis(
                    Random.Range(-CurrentWeapon.spreadAngle, CurrentWeapon.spreadAngle),
                    Camera.main.transform.right) * // Ось X камеры для вертикального отклонения
                    Quaternion.AngleAxis(
                    Random.Range(-CurrentWeapon.spreadAngle, CurrentWeapon.spreadAngle),
                    Camera.main.transform.up); // Ось Y камеры для горизонтального отклонения
                    direction = rotation * direction;
                }
                LaunchBullet(Camera.main.transform.position, direction, CurrentWeapon);
            }
        }

        if (CurrentWeapon != null && CurrentWeapon.behaviuor != null)
        {
            CurrentWeapon.behaviuor.OnShoot();
        }
        onShoot?.Invoke();
    }
    public void LaunchBullet(Vector3 origin, Vector3 dir, WeaponData weapon)
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
            CreateBulletTrail(origin, bulletEndPos, weapon.trailPrefab);
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
                    bool isHeadshot = hit.transform.tag == "Head";
                    hittedChar.ApplyBulletDamage(new DamageInfo() { appliedDamage = weapon.damage, point = hit.point, dir = dir, source = this, headshot = isHeadshot });
                    if (weapon.behaviuor != null && weapon.behaviuor.effectOnProjHit != null && hittedChar.upgrades != null)
                    {
                        hittedChar.upgrades.AddUpgrade(weapon.behaviuor.effectOnProjHit);
                    }
                    CreateBulletTrail(origin, hit.point, weapon.trailPrefab);
                    CreateExplosion(hit.point);
                    onHit?.Invoke(hittedChar.health.CurrentHealth / hittedChar.health.MaxHealth);
                }
            }
            HitInfo info = new();
            info.reciever = hittedChar;
            info.source = _character;
            info.damage = weapon.damage;
            info.dir = dir;
            info.raycastHit = hit;
            info.weapon = weapon;

            foreach (var item in _handlers)
            {
                item.OnHit(info);
            }

            if (hit.transform.tag == "Oil")
            {
                hit.transform.GetComponent<OilController>().FireUp();
                CreateExplosion(bulletEndPos);
                CreateBulletTrail(origin, hit.point, weapon.trailPrefab);
                break;
            }

            //пуля попала во что то кроме персонажа
            if (hittedChar == null)
            {
                if (_currentRicochets < RicochetQuantity)
                {
                    _currentRicochets++;
                    CreateBulletTrail(origin, hit.point, weapon.trailPrefab);
                    LaunchBullet(hit.point, Vector3.Reflect(dir.normalized, hit.normal.normalized), weapon);
                }
                else
                {
                    bulletEndPos = hit.point;
                    CreateExplosion(bulletEndPos);
                    CreateBulletTrail(origin, hit.point, weapon.trailPrefab);
                    CreateBulletHole(hit);
                }
                break;
            }

            if (_currentPierced >= PierceQuantity)
            {
                bulletEndPos = hit.point;
                CreateExplosion(bulletEndPos);
                CreateBulletTrail(origin, hit.point, weapon.trailPrefab);
                break;
            }
            else
            {
                _currentPierced++;
            }
        }
    }

    public void SelectWeapon(int num)
    {
        CurrentModel.gameObject.SetActive(false);
        if (CurrentWeapon != null && CurrentWeapon.behaviuor != null)
        {
            CurrentWeapon.behaviuor.OnDeactivated();
        }
        _selectedWeaponNum = num;

        CurrentModel.gameObject.SetActive(true);

        CurrentWeapon.behaviuor?.OnActivated();

        UpdateAimSettings();
    }
    void ReorderHandlers(Upgrade u)
    {
        _handlers.OrderBy((h) => h.Priority);
    }


    void CreateExplosion(Vector3 pos)
    {
        //GameObject explosion = Instantiate(_explosionPrefab, pos, Quaternion.LookRotation(transform.position - pos));
        //Destroy(explosion, 2f);
    }
    void CreateBulletHole(RaycastHit hitInfo)
    {
            _bulletSurfaceImpact.Execute(hitInfo, CurrentWeapon.holeOnHitWall, CurrentWeapon.particlesOnHitWall);
        
    }
    void CreateBulletTrail(Vector3 startPosition, Vector3 endPosition, ParticleSystem trailPrefab)
    {
        startPosition += originOffset;
        if (CurrentWeapon.haveTrail)
        {
            ParticleLineBuilder.Create(startPosition, endPosition, trailPrefab);
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
                    SelectWeapon(num);
                }
            }
        }
    }
    void UpdateAimSettings()
    {
        _aimController.SetAbilityToAim(CurrentWeapon.ableToAim);
    }
    IEnumerator Reload()
    {

        isReloading = true;
        onReloadStart?.Invoke();
        yield return new WaitForSeconds(CurrentWeapon.reloadTime);

        if (CurrentWeaponState.stockAmmo == -1)
        {
            CurrentWeaponState.loadedAmmo = CurrentWeapon.maxLoadedAmmo;
        }
        else
        {
            int reducedAmmo = CurrentWeaponState.stockAmmo > CurrentWeapon.maxLoadedAmmo ? CurrentWeapon.maxLoadedAmmo : CurrentWeaponState.stockAmmo;

            CurrentWeaponState.loadedAmmo = reducedAmmo;
            CurrentWeaponState.stockAmmo -= reducedAmmo;
        }
        isReloading = false;
        onReloadEnd?.Invoke();
    }
}
