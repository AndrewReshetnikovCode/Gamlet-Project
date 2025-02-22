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
    public UpgradeController upgrade;
    public Brain brain;
    public NavMeshAgentMovement navMeshMovement;
    public CharacterRow row;
    public Spawner spawner;
    public Rigidbody rb;
    public VisualEffectController visualEffect;
    public ShootingController shooting;
    public Animator animator;

    public Transform mainTransform;

    float _rtp;
    public float RangeToPlayer => _rtp;

    [AutoAssignStat] Stat _money;
    [AutoAssignStat] Stat _blood;
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
        if (upgrade == null)
            upgrade = GetComponent<UpgradeController>();
        if (visualEffect == null)
            visualEffect = GetComponent<VisualEffectController>();
        if (shooting == null)
            shooting = GetComponent<ShootingController>();
        if (animator == null)
            animator = GetComponent<Animator>();
        if (mainTransform == null)
            mainTransform = transform;
        if (itemsCollection == null)
            itemsCollection = GetComponent<ItemsCollection>();
    }
    private void Start()
    {
        CharacterManager.Instance.OnCharacterInit(this);
    }
    private void Update()
    {
        _rtp = Vector3.Distance(GetMainTransform().position,PlayerManager.Instance.Player.transform.position);
    }
    public void Init()
    {
        stats?.Init();
        health?.Init();
        brain?.Init();
    }

    public void ApplyDamage(float v, object s)
    {
        health.ApplyDamage(v, s);
    }
    public void BindToSpawner(Spawner spawner)
    {

    }
    public void OnEnterFight()
    {
        onEntersFight?.Invoke();
    }
    public void OnExitFight()
    {
        onExitFight?.Invoke();
    }
    public Transform GetMainTransform()
    {
        return mainTransform;
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
        if(type == CurrencyType.Money)
        {
            _money.AddBaseValue(amount, true);
        }
        else
        {
            _blood.AddBaseValue(amount, true);
        }
    }
}
public enum CurrencyType
{
    Money,
    Blood
}
