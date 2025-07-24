using UnityEngine;

public class ChildNotifierOnDestroy : MonoBehaviour
{
    DummyRespawn _containerMonitor;

    // Метод инициализации для задания ссылки на родительский ContainerMonitor
    public void Init(DummyRespawn containerMonitor)
    {
        _containerMonitor = containerMonitor;
    }

    void OnDestroy()
    {
        // Если _containerMonitor существует, уведомляем его об уничтожении этого объекта
        if (_containerMonitor != null)
        {
            _containerMonitor.NotifyObjectDestroyed(gameObject);
        }
    }
}