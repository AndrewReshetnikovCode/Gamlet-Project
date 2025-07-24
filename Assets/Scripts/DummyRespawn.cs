using UnityEngine;
using System;

public class DummyRespawn : MonoBehaviour
{
    // ��������� ���� ��� ������� ���������� (��� � ��������� �����)
    public Transform prefab;

    // ��������� �������� ��� �������� ��������� ���������� (��� � ������� �����)
    public Transform CurrentContainer { get => _currentContainer; private set => _currentContainer = value; }

    // ������� (���������� � on)
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
        // ���� �� �����-�� ������� ��������� ����������� � �������
        if (CurrentContainer == null)
            return;

        // ���������, �������� �� � ���������� ���� �� ���� �� ������������ ������
        bool anyAlive = false;
        foreach (Transform child in CurrentContainer.transform)
        {
            // ���� ������ �� GameObject ����� null, ������ ���������
            if (child != null && child.gameObject != null)
            {
                anyAlive = true;
                break;
            }
        }

        if (!anyAlive)
        {
            // ���� ��� �������� ������� ����������, ������ ����� ���������
            InstantiateContainer();
        }
    }
    // ���� ����� ���������� �� ChildNotifier ��� ����������� ��������� �������
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
        // ������ ����� ��������� ���������� �� �������
        Transform newContainer = Instantiate(prefab, _mainContainer);
        CurrentContainer = newContainer; // ��������� ��������� ��������

        // ��������� ��������, ���� � ������ ���������� ���� Animator
        Animator anim = newContainer.GetComponent<Animator>();
        if (anim != null)
        {
            // �������� �������� ������ "StartAnimation". ��� ����� �������� �� �������������.
            anim.SetTrigger("Spawn");
        }

        CurrentContainer.gameObject.SetActive(true);
        // �������� ������� �������� ������ ����������
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

