using UnityEngine;
using DemiurgEngine.StatSystem;
using System;
using System.Collections;
using UnityEngine.Events;
using Unity.VisualScripting;

[RequireComponent(typeof(StatsController))]
public class HealthController : MonoBehaviour
{
    [SerializeField] float _defaultKnockOutTime = 1f;

    public event Action<float> onHealthChanged;
    public event Action<float, object> onDamageApplied;
    public UnityEvent OnDamageApplied;


    DeathController _deathHandler;

    StatsController _sc;

    [AutoAssignStat]
    Stat _health;
    public float CurrentHealth { get => _health.CurrentValue; set { _health.SetCurrentValue(value, true); } }
    public float MaxHealth { get => _health.BaseValue; set { _health.SetBaseValue(value, true); } }

    [SerializeField] float _invulnerabilityTime;
    float _lastAttackTime;

    [SerializeField] float _currentHealthValue;
     
    public void Init()
    {
        _deathHandler = GetComponent<DeathController>();
        _sc = GetComponent<StatsController>();

        _health.onChange += OnHealthChange;
    }
    [SerializeField] float _regenInterval;
    float _lastRegenTime;
    [Tooltip("From 1 to 100")]
    [SerializeField] float _regenPercent;
    private void Update()
    {
        if (Time.time - _lastRegenTime > _regenInterval)
        {
            _health.AddCurrentValue(_health.BaseValue * (_regenPercent/100), true);
            _lastRegenTime = Time.time;
        }
        _currentHealthValue = _health.CurrentValue;
    }
    void OnHealthChange(float current, float max)
    {
        onHealthChanged?.Invoke(current);
        _currentHealthValue = current;
    }
    public void ApplyDamage(float damage, object source)
    {
        if (Time.time - _lastAttackTime < _invulnerabilityTime)
        {
            return;
        }
        _lastAttackTime = Time.time;

        if (damage >= CurrentHealth)
        {
            CurrentHealth = 0;
        }
        else
        {
            CurrentHealth -= damage;
        }

        onDamageApplied?.Invoke(_health.CurrentValue, source);
        OnDamageApplied?.Invoke();
        EventBus.Trigger<(Vector3, float)>("OnDamage", (transform.position, damage));
        //gameObject.GetComponentInParent<DamageAnimation>()?.PlayAnimation(transform.gameObject);

        if (CurrentHealth == 0)
        {
            OnZeroHP(source);
        }
    }

    public virtual void OnZeroHP(object source)
    {
         _deathHandler.Die();
    }
    public void KnockOut(float time)
    {
        StartKnockOut();
        StartCoroutine(KnockOutRoutine(time));
    }
    public void KnockOut()
    {
        StartKnockOut();
        StartCoroutine(KnockOutRoutine(_defaultKnockOutTime));
    }
    void StartKnockOut()
    {
        CharacterFacade c = GetComponent<CharacterFacade>();
        
    }
    void EndKnockOut()
    {

    }
    IEnumerator KnockOutRoutine(float t)
    {
        yield return new WaitForSeconds(t);
        EndKnockOut();
    }
}
