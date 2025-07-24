using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public enum PoolType
{
    ObjectPool,
    LinkedPool
}

public class GameObjectPool
{
    public GameObject Prefab { get => _prefab; set => _prefab = value; }

    [SerializeField] PoolType _poolImplementation = PoolType.ObjectPool;
    [SerializeField] bool _collectionChecks = true;
    [SerializeField] int _maxPoolSize = 100;
    [SerializeField] GameObject _prefab;

    public Action<GameObject> onGet;
    public Action<GameObject> onRelease;
    public Action<GameObject> onDestroy;

    IObjectPool<GameObject> _pool;
    List<GameObject> _activated = new(100);
    List<GameObject> _deactivated = new(100);



    List<GameObject> _objsToAdd = new();
    GameObject _currentObjectToAdd = null;
    bool _interateObjectsToAdd = true;
    public GameObjectPool(List<GameObject> objs)
    {
        _objsToAdd = objs;
        InitializePool();
    }
    public GameObjectPool()
    {
        InitializePool();
    }
    void InitializePool()
    {

        switch (_poolImplementation)
        {
            case PoolType.ObjectPool:
                _pool = new ObjectPool<GameObject>(
                   CreateFunc,
                   InternalOnGet,
                   InternalOnRelease,
                   InternalOnDestroy,
                   _collectionChecks,
                   defaultCapacity: 10,
                   _maxPoolSize);
                break;
            case PoolType.LinkedPool:
                _pool = new LinkedPool<GameObject>(
                   CreateFunc,
                   InternalOnGet,
                   InternalOnRelease,
                   InternalOnDestroy,
                   _collectionChecks,
                   _maxPoolSize);
                break;
            default:
                Debug.LogError("Неизвестная реализация пула!");
                break;
        }


        for (int i = 0; i < _objsToAdd.Count; i++)
        {
            _interateObjectsToAdd = true;

            _currentObjectToAdd = _objsToAdd[i];
            _currentObjectToAdd.AddComponent<ReturnToPool>().SetPoolManager(this);
            _pool.Get(out _currentObjectToAdd);
        }
        _interateObjectsToAdd = false;
        _currentObjectToAdd = null;
    }
    GameObject CreateFunc()
    {
        if (_prefab == null && _interateObjectsToAdd == false)
        {
            Debug.LogWarning("Префаб не задан!");
        }
        if (_interateObjectsToAdd)
        {
            return _currentObjectToAdd;
        }
        else
        {

            GameObject instance = GameObject.Instantiate(_prefab);
            ReturnToPool returnComponent = instance.GetComponent<ReturnToPool>();
            if (returnComponent == null)
            {
                returnComponent = instance.gameObject.AddComponent<ReturnToPool>();
            }
            returnComponent.SetPoolManager(this);

            _activated.Add(instance);
            return instance;
        }
    }
    void ResetGameObject(GameObject obj)
    {
        obj.GetComponent<PooledGameObject>().ResetPooled();   
    }
    void InternalOnGet(GameObject obj)
    {
        ResetGameObject(obj);

        _deactivated.Remove(obj);
        if (_activated.Contains(obj) == false)
        {
            _activated.Add(obj);
        }
        onGet?.Invoke(obj);
    }

    void InternalOnRelease(GameObject obj)
    {
        _activated.Remove(obj);
        if (_deactivated.Contains(obj) == false)
        {
            _deactivated.Add(obj);
        }
        onRelease?.Invoke(obj);
    }

    void InternalOnDestroy(GameObject obj)
    {
        _activated.Remove(obj);
        onDestroy?.Invoke(obj);
    }
    public int ActiveCount => _activated.Count;
    public int InactiveCount => _deactivated.Count;
    public GameObject GetActive(int num)
    {
        return _activated[num];
    }
    public GameObject GetInactive(int num)
    {
        return _deactivated[num];
    }

    public GameObject Get()
    {
        GameObject instance = _pool.Get();
        if (instance != null)
            instance.gameObject.SetActive(true);
        return instance;
    }

    public void Release(GameObject instance)
    {
        if (instance != null)
        {
            _pool.Release(instance);
            //instance.gameObject.SetActive(false);
        }
    }

}


