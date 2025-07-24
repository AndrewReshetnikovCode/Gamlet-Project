using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradesController : MonoBehaviour
{
    public event Action<Upgrade> OnUpgradeAdded;
    public event Action<Upgrade> OnUpgradeRemoved;

    public class UpgradeState
    {
        public float tickTimer;
    }
    class Bundle
    {
        public Upgrade upgrade;
        public UpgradeState state;
    }

    public CharacterFacade character;

    public int maxUpgrades = 5;

    [SerializeField] List<Upgrade> _upgradesAtStart;

    [SerializeField] List<Bundle> _activeUpgrades = new();

    [SerializeField] VisualEffectsController _visualEffets;

    public void Init()
    {
        character = GetComponent<CharacterFacade>();
        if (_upgradesAtStart != null)
        {
            foreach (var item in _upgradesAtStart)
            {
                AddUpgrade(item);
            }
        }

        character.death.OnDeath.AddListener(OnDeath);
    }
    private void Update()
    {
        for (int i = 0; i < _activeUpgrades.Count; i++)
        {
            Upgrade upgrade = _activeUpgrades[i].upgrade;
            UpgradeState state = _activeUpgrades[i].state;

            state.tickTimer -= Time.deltaTime;
            if (state.tickTimer <= 0)
            {
                upgrade.Tick(this);
                state.tickTimer = upgrade.tickInterval;
            }
        }
    }
    private void OnDestroy()
    {
        foreach (var item in _activeUpgrades)
        {
            item.upgrade.Deactivate(this);
        }
    }

    public void Apply(Upgrade logic, VisualEffects visual)
    {
        _visualEffets.SetActive(visual, true);
        AddUpgrade(logic);
    }
    public void ApplyTemporary(Upgrade logic, float time, VisualEffects visual)
    {
        _visualEffets.SetActive(visual, true);
        AddUpgrade(logic);

        StartCoroutine(DisableEffect(logic, time, visual));
    }
    public bool AddUpgrade(Upgrade newUpgrade, bool resetTimerIfExistsSame = true)
    {
        Bundle existingUpgrade = _activeUpgrades.Find(b => b.upgrade.upgradeName == newUpgrade.upgradeName);

        if (existingUpgrade != null)
        {
            return false;
        }

        if (_activeUpgrades.Count >= maxUpgrades)
            return false;

        Add(newUpgrade);

        
        return true;
    }

    public void RemoveUpgrade(Upgrade u)
    {
        Bundle finded = _activeUpgrades.Find(b => b.upgrade == u);
        if (finded == null)
        {
            return;
        }
        _activeUpgrades.Remove(finded);

        finded.upgrade.Deactivate(this);
        OnUpgradeRemoved?.Invoke(finded.upgrade);
    }
    
    public bool HasUpgrade(Upgrade upgrade)
    {
        return _activeUpgrades.Exists(b => b.upgrade);
    }
    public bool HasUpgrade<T>()
    {
        Type t = typeof(T);
        return _activeUpgrades.Find(u => u.upgrade.GetType() == t) != null;
    }
    public UpgradeState GetState(Upgrade upgrade)
    {
        Bundle b = _activeUpgrades.Find(b => b.upgrade == upgrade);
        return b == null ? null : b.state;
    }
    void Add(Upgrade u)
    {
        Bundle bundle = new();
        bundle.upgrade = u;
        bundle.state = new UpgradeState() { tickTimer = u.tickInterval };

        _activeUpgrades.Add(bundle);

        u.Activate(this);
        OnUpgradeAdded?.Invoke(u);
    }
    void OnDeath()
    {
        foreach (var item in _activeUpgrades)
        {

        }
    }
    IEnumerator DisableEffect(Upgrade effectLogic, float delay, VisualEffects visual)
    {
        yield return new WaitForSeconds(delay);
        _visualEffets.SetActive(visual, false);
        RemoveUpgrade(effectLogic);
    }
}
