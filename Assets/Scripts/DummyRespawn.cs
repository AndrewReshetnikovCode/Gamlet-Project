using UnityEngine;
using System;

public class DummyRespawn : MonoBehaviour
{
    // Публичное поле для префаба контейнера (имя с маленькой буквы)
    public Transform prefab;

    // Публичное свойство для текущего активного контейнера (имя с большой буквы)
    public Transform CurrentContainer { get => _currentContainer; private set => _currentContainer = value; }

    // События (начинаются с on)
    public event Action onContainerInstantiated;
    public event Action<GameObject> onObjectDestroyed;

    [SerializeField] Transform _currentContainer;
    [SerializeField] Transform _mainContainer;
    void Start()
    {
        if (_currentContainer == null)
        {
            InstantiateContainer();
        }
        InitNotifiers();
    }

    void Update()
    {
        // Если по какой-то причине контейнер отсутствует – выходим
        if (CurrentContainer == null)
            return;

        // Проверяем, остались ли в контейнере хотя бы один не уничтоженный объект
        bool anyAlive = false;
        foreach (Transform child in CurrentContainer.transform)
        {
            // Если ссылка на GameObject равна null, объект уничтожен
            if (child != null && child.gameObject != null)
            {
                anyAlive = true;
                break;
            }
        }

        if (!anyAlive)
        {
            // Если все дочерние объекты уничтожены, создаём новый контейнер
            InstantiateContainer();
        }
    }
    // Этот метод вызывается из ChildNotifier при уничтожении дочернего объекта
    public void NotifyObjectDestroyed(GameObject obj)
    {
        onObjectDestroyed?.Invoke(obj);
    }
    public void ForceRespawn()
    {
        InstantiateContainer();
    }
    void InstantiateContainer()
    {
        if (prefab == null)
        {
            return;
        }
        Destroy(CurrentContainer.gameObject);
        // Создаём новый экземпляр контейнера из префаба
        Transform newContainer = Instantiate(prefab, _mainContainer);
        CurrentContainer = newContainer; // обновляем публичное свойство

        // Запускаем анимацию, если у нового контейнера есть Animator
        Animator anim = newContainer.GetComponent<Animator>();
        if (anim != null)
        {
            // Параметр анимации назван "StartAnimation". Его можно изменить по необходимости.
            anim.SetTrigger("Spawn");
        }

        CurrentContainer.gameObject.SetActive(true);
        // Вызываем событие создания нового контейнера
        onContainerInstantiated?.Invoke();
    }
    void InitNotifiers()
    {
        foreach (Transform child in CurrentContainer.transform)
        {
            ChildNotifierOnDestroy notifier = child.GetComponent<ChildNotifierOnDestroy>();
            if (notifier == null)
            {
                notifier = child.gameObject.AddComponent<ChildNotifierOnDestroy>();
            }
            notifier.Init(this);
        }
    }
    
}

