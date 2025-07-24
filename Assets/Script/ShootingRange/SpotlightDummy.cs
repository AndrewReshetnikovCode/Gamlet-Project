using UnityEngine;

public class SpotlightDummy : MonoBehaviour
{
    [SerializeField] Transform _toRotate;
    [SerializeField] Transform _arrowPointer;
    [SerializeField] Vector3 _arrowOffset;
    // Публичное поле для целевого Transform (имя с маленькой буквы)
    public Transform target;
    // Публичное поле для ссылки на монитор контейнера
    public DummyRespawn containermonitor;

    void Update()
    {
        _toRotate.gameObject.SetActive(true);
        _arrowPointer.gameObject.SetActive(true);
        // Если задан контейнер, проверяем наличие дочерних объектов
        if (containermonitor != null && containermonitor.CurrentContainer != null)
        {
            int childCount = containermonitor.CurrentContainer.transform.childCount;
            if (childCount == 0)
            {
                _toRotate.gameObject.SetActive(false);
                _arrowPointer.gameObject.SetActive(false);
                return;
            }
            // Если целевой Transform уничтожен или не назначен, выбираем случайный дочерний объект
            if (target == null)
            {
                int index = Random.Range(0, childCount);
                target = containermonitor.CurrentContainer.transform.GetChild(index);
            }
        }

        // Если целевой Transform существует, наводим forward объекта на него
        if (target != null)
        {
            Vector3 direction = target.position - _toRotate.position;
            if (direction != Vector3.zero)
            {
                _toRotate.forward = direction;
            }
            _arrowPointer.position = target.position + _arrowOffset;
        }
    }
}
