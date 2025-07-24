using DemiurgEngine;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using EventBus = Unity.VisualScripting.EventBus;
//using static UnityEngine.Rendering.DebugUI.Table;


public class DeathController : MonoBehaviour
{
    public UnityEvent OnDeath;

    public bool playAnimOnAlive; 
    public string aliveAnimTrigger;

    [SerializeField] HealthController _health;
    [SerializeField] AudioClip _deathSound;
    [SerializeField] float _destroyDelay;

    public bool Dead => _health.CurrentHealth == 0;

    public void Die()
    {
        CharacterFacade character = GetComponent<CharacterFacade>();

        
        Invoke(nameof(DestroyGameObject), _destroyDelay);

        character.spawner?.OnCreatureDeath(gameObject);
        ProtectedOnDeath();
        OnDeath?.Invoke();
    }
    //protected virtual void ExecuteActions()
    //{
    //    PlaySound();
    //}
    public void MakeAlive()
    {
        GetComponent<HealthController>().CurrentHealth = GetComponent<HealthController>().MaxHealth;
        if (playAnimOnAlive)
        {
            GetComponentInChildren<Animator>().SetTrigger(aliveAnimTrigger);
        }
    }
    void DestroyGameObject() { Destroy(GetComponent<CharacterFacade>().GetMainTransform().gameObject); }
    protected virtual void ProtectedOnDeath()
    {

    }
}
