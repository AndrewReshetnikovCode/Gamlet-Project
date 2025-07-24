using Assets.StatSystem;
using DemiurgEngine.StatSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public event Action<bool> onTrueSightChange;

    public static PlayerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new();
                go.name = nameof(PlayerManager);
                go.AddComponent<PlayerManager>();
                return _instance;
            }
            else
            {
                return _instance;
            }

        }
    }

    public GameObject Player { get => _player; set => _player = value; }
    public bool TrueSightActive { get => _ts; set { if (value != _ts) onTrueSightChange?.Invoke(value); _ts = value; } }
    public float TrueSightRange => _trueSightRange.BaseValue;
    public CharacterFacade Character { get; private set; }
    public static CharacterFacade CharacterStatic => Instance.Character;
    public bool CursorControl 
    {   
        get => _cl; 
        set 
        { 
            _cl = value; 
            Cursor.visible = value; 
            Character.shooting.enabled = !value; 
            _cameraRotation.enabled = !value;
            _aim.enabled = !value;
            Cursor.lockState = value ? CursorLockMode.Confined : CursorLockMode.Locked; 
        } 
    }
    public bool MovementLocked 
    { 
        get => !_movement.enabled; 
        set => _movement.enabled = !value; 
    }

    static PlayerManager _instance;
    [SerializeField] GameObject _player;
    [SerializeField] UIInventoryManager _inventoryManager;
    [SerializeField] PlayerMovement _movement;
    [SerializeField] PlayerCamera _cameraRotation;
    [SerializeField] AimFovController _aim;
    Stat _trueSightRange => Character.stats.GetStat("TSRange");
    bool _ts;
    bool _timeScaptApplied = false;
    bool _cl;
    void Awake()
    {
        _instance = this;
        if (_player == null)
        {
            _player = GameObject.FindWithTag("Player");
            if (_player == null)
            {
                Debug.LogError("No player on scene!");
                return;
            }
        }
        Character = _player.GetComponent<CharacterFacade>();
    }
    public void SetTimeScale(float scale, bool revert, float revertDelay)
    {
        if (_timeScaptApplied)
        {
            StopAllCoroutines();
        }
        Time.timeScale = scale;
        _timeScaptApplied = true;
        if (revert)
        {
            StartCoroutine(RevertTimeScaleRoutine(revertDelay * scale));
        }
    }
    public void OpenInventory(bool withTrader)
    {
        CursorControl = true;
        MovementLocked = true;

        _inventoryManager.SetTraderWindowActive(withTrader);
        _inventoryManager.Display(true);
    }
    public void CloseInventory()
    {
        CursorControl = false;
        MovementLocked = false;

        _inventoryManager.Display(false);
    }
    IEnumerator RevertTimeScaleRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        Time.timeScale = 1f;
        _timeScaptApplied = false;
    }
}
