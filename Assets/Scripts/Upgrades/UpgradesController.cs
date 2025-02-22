using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    public event Action<Upgrade> OnUpgradeAdded;
    public event Action<Upgrade> OnUpgradeRemoved;

    public CharacterFacade character;

    [Header("Upgrades Settings")]
    public int maxUpgrades = 5;

    private List<Upgrade> activeUpgrades = new List<Upgrade>();
    private List<TemporaryUpgrade> activeTemporaryUpgrades = new();
    private Dictionary<Upgrade, UpgradeState> upgradeTimers = new();
    public Dictionary<Upgrade, ActionOnTick> upgradesOnTickActions = new();
    public class ActionOnTick
    {
        public object source;
        public Action<(Upgrade,object,UpgradeController)> action;
    }
    class UpgradeState
    {
        public float tickTimer;
        public float lifetimeTimer;
    }

    public List<Upgrade> upgradesAtStart;
    private void Start()
    {
        character = GetComponent<CharacterFacade>();
        if (upgradesAtStart != null)
        {
        foreach (var item in upgradesAtStart)
        {
                AddUpgrade(item);
        }

        }
    }
    private void Update()
    {
        // Обновляем таймеры для каждого апгрейда
        Upgrade[] u = activeUpgrades.ToArray();
        for (int i = 0; i < u.Length; i++)
        {
            Upgrade upgrade = u[i];
            UpgradeState timers = upgradeTimers[upgrade];
            timers.tickTimer -= Time.deltaTime;
            if (timers.tickTimer <= 0)
            {
                
                upgrade.Tick(this);
                timers.tickTimer = upgrade.tickInterval;
            }
        }
        TemporaryUpgrade[] t = activeTemporaryUpgrades.ToArray();
        for (int i = 0; i < t.Length; i++)
        {
            TemporaryUpgrade upgrade = t[i];
            UpgradeState timers = upgradeTimers[upgrade];
            timers.tickTimer -= Time.deltaTime;
            timers.lifetimeTimer -= Time.deltaTime;
            if (upgradeTimers[upgrade].tickTimer <= 0)
            {
                upgrade.Tick(this);
                if (upgradesOnTickActions.ContainsKey(upgrade))
                {
                    ActionOnTick a = upgradesOnTickActions[upgrade];
                    a.action?.Invoke((upgrade, a.source, this));

                }

                timers.tickTimer = upgrade.tickInterval;
            }
            if (timers.lifetimeTimer < 0)
            {
                RemoveUpgrade(upgrade);
            }
        }
    }
    private void OnDestroy()
    {
        foreach (var item in activeUpgrades)
        {
            item.Deactivate(this);
        }
        foreach (var item in activeTemporaryUpgrades)
        {
            item.Deactivate(this);

        }
    }

    public bool AddUpgrade(Upgrade newUpgrade)
    {
        // Проверяем, есть ли апгрейд этого типа
        Upgrade existingUpgrade = activeUpgrades.Find(u => u.GetType() == newUpgrade.GetType());
        if (upgradeTimers.ContainsKey(newUpgrade) == false)
        {
            upgradeTimers.Add(newUpgrade, new UpgradeState());
        }
        if (existingUpgrade != null)
        {
            TemporaryUpgrade t = newUpgrade as TemporaryUpgrade;
            if (t != null)
            {
                upgradeTimers[existingUpgrade].lifetimeTimer += t.lifetime;
                return true;
            }
            else
            {
                if (existingUpgrade.strongerVersion == newUpgrade)
                {
                    RemoveUpgrade(existingUpgrade);
                    Add(newUpgrade);

                    upgradeTimers[newUpgrade].tickTimer = newUpgrade.tickInterval;
                    newUpgrade.Activate(this);
                    OnUpgradeAdded?.Invoke(newUpgrade);
                    return true;
                }
                return false; // Не удалось добавить
            }
        }

        // Проверяем лимит
        if (activeUpgrades.Count >= maxUpgrades)
            return false;

        // Добавляем новый апгрейд
        Add(newUpgrade);
        upgradeTimers[newUpgrade].tickTimer = newUpgrade.tickInterval;
        newUpgrade.Activate(this);
        OnUpgradeAdded?.Invoke(newUpgrade);
        return true;
    }

    public void RemoveUpgrade(Upgrade u)
    {
        if (u is TemporaryUpgrade)
        {
            activeTemporaryUpgrades.Remove((TemporaryUpgrade)u);
        }
        else
        {
            activeUpgrades.Remove(u);
        }
        u.Deactivate(this);
        OnUpgradeRemoved?.Invoke(u);
    }
    void Add(Upgrade u)
    {
        if (u is TemporaryUpgrade)
        {
            activeTemporaryUpgrades.Add((TemporaryUpgrade)u);
            upgradeTimers[u].lifetimeTimer = ((TemporaryUpgrade)u).lifetime;
        }
        else
        {
            activeUpgrades.Add(u);
        }
    }
    public bool HasUpgrade(Upgrade upgrade)
    {
        return activeUpgrades.Contains(upgrade);
    }
    public bool HasUpgrade<T>()
    {
        Type t = typeof(T);
        return activeUpgrades.Find(u => u.GetType() == t) != null;
    }
}
