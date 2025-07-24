using DemiurgEngine.StatSystem;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Spawner : MonoBehaviour
{
    [SerializeField] string _poolId;
    [SerializeField] GameObject _creaturePrefab;
    [SerializeField] int _maxCreatures;
    [SerializeField] float _delay;
    int _aliveCount;
    bool _spawnIsActive = false;
    SpawnerManager _sm;
    StatsController _sc;
    [SerializeField] private float minSpawnRadius = 5f; // Минимальный радиус появления
    [SerializeField] private float maxSpawnRadius = 10f; // Максимальный радиус появления
    public StatsController StatsController { get => _sc; set => _sc = value; }

    bool _initialized = false;
    private void Start()
    {
        if (_initialized == false)
        {
            Init(SpawnerManager.instance);

        }
    }
    public void Init(SpawnerManager sm)
    {
        _sc = GetComponent<StatsController>();

        _sm = sm;

        StartSpawn();

        _initialized = true;
    }

    public void OnCreatureDeath(GameObject creature)
    {
        _aliveCount--;
    }
    protected virtual bool AbleToSpawn()
    {
        return _aliveCount <= _maxCreatures;
    }
    void Spawn()
    {
        // Вычисляем случайное расстояние от игрока в пределах заданного радиуса
        float spawnDistance = Random.Range(minSpawnRadius, maxSpawnRadius);

        // Выбираем случайный угол для размещения врага
        float spawnAngle = Random.Range(0f, 360f);
        Vector3 spawnDirection = new Vector3(Mathf.Sin(spawnAngle), 0f, Mathf.Cos(spawnAngle));

        RaycastHit hit;
        Vector3 pos;
        if (Physics.Raycast(transform.position, spawnDirection, out hit, spawnAngle, LayerMask.GetMask("Default")))
        {
            pos = hit.point;
        }
        else
        {
            pos = transform.position + spawnDirection * spawnDistance;
        }

        CharacterFacade newChar = PoolManager.CreatePooledItem(_creaturePrefab, pos, Quaternion.identity, null).GetComponentInChildren<CharacterFacade>();

        newChar.BindToSpawner(this);
        _aliveCount++;
    }
    public void StartSpawn()
    {
        StartCoroutine(SpawnRoutine());
    }
    public void StopSpawn()
    {
        StopAllCoroutines();
    }
    IEnumerator SpawnRoutine()
    {
        _spawnIsActive = true;
        while (_spawnIsActive)
        {
            if (AbleToSpawn())
            {
                Spawn();
            }
            yield return new WaitForSeconds(_delay);
        }
    }
}
