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

    [SerializeField] AudioClip _deathSound;
    [SerializeField] float _destroyDelay;

    public bool Dead { get; private set; } = false;

    public void Die()
    {
        
        Dead = true;
        CharacterFacade character = GetComponent<CharacterFacade>();
        CharacterRow creatureRow = character.row;
        if (creatureRow != null)
        {
            creatureRow.isAlive.Value = false;
            creatureRow.spawner.Value?.OnCreatureDeath(creatureRow.gameObject.Value);
        }
        //ExecuteActions();
        if (tag != "Player")
        {
            Invoke(nameof(DestroyGameObject), _destroyDelay);
        }
        else
        {
            gameObject.SetActive(false);
        }
        //GetComponentInChildren<TMP_Text>().text = "Dead";

        ProtectedOnDeath();
        OnDeath?.Invoke();
    }
    //protected virtual void ExecuteActions()
    //{
    //    PlaySound();
    //}
    
    void DestroyGameObject() { Destroy(GetComponent<CharacterFacade>().GetMainTransform().gameObject); }
    protected virtual void ProtectedOnDeath()
    {

    }
}
