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
    static PlayerManager _instance;
    [SerializeField] GameObject _player;

    public GameObject Player { get => _player; set => _player = value; }
    bool _ts;
    public bool TrueSightActive { get => _ts; set { if (value != _ts) onTrueSightChange?.Invoke(value); _ts = value; } }
    Stat _trueSightRange => Character.stats.GetStat("TSRange");
    public float TrueSightRange => _trueSightRange.BaseValue;
    public CharacterFacade Character { get; private set; }
    public static CharacterFacade StCharacter => Instance.Character;
    bool _timeScaptApplied = false;
    bool _cl;
    public bool CursorLocked { get => _cl; set { _cl = value; Cursor.visible = !value; Character.shooting.enabled = value; Camera.main.GetComponentInParent<PlayerCamera>().enabled = value; Cursor.lockState = value ? CursorLockMode.Locked:CursorLockMode.Confined; } }
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
    IEnumerator RevertTimeScaleRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        Time.timeScale = 1f;
        _timeScaptApplied = false;
    }
}
