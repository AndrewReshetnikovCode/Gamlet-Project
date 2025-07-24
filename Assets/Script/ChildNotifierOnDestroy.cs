using UnityEngine;

public class ChildNotifierOnDestroy : MonoBehaviour
{
    DummyRespawn _containerMonitor;

    // ����� ������������� ��� ������� ������ �� ������������ ContainerMonitor
    public void Init(DummyRespawn containerMonitor)
    {
        _containerMonitor = containerMonitor;
    }

    void OnDestroy()
    {
        // ���� _containerMonitor ����������, ���������� ��� �� ����������� ����� �������
        if (_containerMonitor != null)
        {
            _containerMonitor.NotifyObjectDestroyed(gameObject);
        }
    }
}