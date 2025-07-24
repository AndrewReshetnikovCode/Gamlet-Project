using DemiurgEngine.StatSystem;
using InventorySystem;
using System;
using UnityEngine;


public class CharacterFacade : MonoBehaviour
{
    public event Action onEntersFight;
    public event Action onExitFight;

    public StatsController stats;
    public ItemsCollection itemsCollection;
    public HealthController health;
    public DeathController death;
    public UpgradesController upgrades;
    public Brain brain;
    public NavMeshAgentMovement navMeshMovement;
    public CharacterRow row;
    public Spawner spawner;
    public Rigidbody rb;
    public VisualEffectsController visualEffect;
    public ShootingController shooting;
    public Animator animator;
    public ScreenAnimationManager screenAnim;

    public Transform rootTransform;

    float _rtp;

    public float RangeToPlayer => _rtp;

    public Transform RootTransform
    {
        get => rootTransform;
    }
    [AutoAssignStat] Stat _money;
    [AutoAssignStat] Stat _blood;

    bool _inited = false;

    CharactersCollection _collection;
    private void Awake()
    {
        if (stats == null)
            stats = GetComponent<StatsController>();
        if (health == null)
            health = GetComponent<HealthController>();
        if (death == null)
            death = GetComponent<DeathController>();
        if (brain == null)
            brain = GetComponent<Brain>();
        if (navMeshMovement == null)
            navMeshMovement = GetComponent<NavMeshAgentMovement>();
        if (rb == null)
            rb = GetComponent<Rigidbody>();
        if (upgrades == null)
            upgrades = GetComponent<UpgradesController>();
        if (visualEffect == null)
            visualEffect = GetComponent<VisualEffectsController>();
        if (shooting == null)
            shooting = GetComponent<ShootingController>();
        if (animator == null)
            animator = GetComponent<Animator>();
        if (rootTransform == null)
            rootTransform = transform;
        if (itemsCollection == null)
            itemsCollection = GetComponent<ItemsCollection>();
        if (screenAnim == null)
            screenAnim = GetComponent<ScreenAnimationManager>();
    }
    private void Start()
    {
        InitComponents();
        

        _inited = true;
    }
    private void Update()
    {
        _rtp = Vector3.Distance(RootTransform().position, PlayerManager.Instance.Player.transform.position);
    }
    private void OnDestroy()
    {
        _collection.Remove(this);
    }
    public void Init(CharactersCollection collection)
    {
        _collection = collection;
    }
    public void ResetValues()
    {
        if (_inited)
        {
            stats.Reset();
        }
    }
    public void InitComponents()
    {
        stats?.Init();
        health?.Init();
        brain?.Init();
        if (tag == "Player")
        {
            screenAnim.Init();
        }
        upgrades?.Init();
    }
    public void ApplyBulletDamage(DamageInfo damageInfo)
    {
        visualEffect?.CreateParticle(damageInfo.point, -damageInfo.dir, 1, ParticleType.Blood);
        ApplyDamage(damageInfo.appliedDamage, damageInfo.source);
    }
    public void ApplyDamage(float v, object s)
    {
        health.ApplyDamage(v, s);
    }
    public void BindToSpawner(Spawner spawner)
    {
        this.spawner = spawner;
    }
    public void OnEnterFight()
    {
        onEntersFight?.Invoke();
    }
    public void OnExitFight()
    {
        onExitFight?.Invoke();
    }
    public void StopMoving()
    {
        navMeshMovement.StopMovement();
    }
    public int GetCurrency(CurrencyType type)
    {
        if (type == CurrencyType.Money)
        {
            return (int)_money.BaseValue;
        }
        else
        {
            return (int)_blood.BaseValue;
        }
    }
    public void AddCurrency(CurrencyType type, int amount)
    {
        if (type == CurrencyType.Money)
        {
            _money.AddBaseValue(amount, true);
        }
        else
        {
            _blood.AddBaseValue(amount, true);
        }
    }
}

