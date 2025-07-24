using UnityEngine;

public class SpotlightDummy : MonoBehaviour
{
    [SerializeField] Transform _toRotate;
    [SerializeField] Transform _arrowPointer;
    [SerializeField] Vector3 _arrowOffset;
    // ��������� ���� ��� �������� Transform (��� � ��������� �����)
    public Transform target;
    // ��������� ���� ��� ������ �� ������� ����������
    public DummyRespawn containermonitor;

    void Update()
    {
        _toRotate.gameObject.SetActive(true);
        _arrowPointer.gameObject.SetActive(true);
        // ���� ����� ���������, ��������� ������� �������� ��������
        if (containermonitor != null && containermonitor.CurrentContainer != null)
        {
            int childCount = containermonitor.CurrentContainer.transform.childCount;
            if (childCount == 0)
            {
                _toRotate.gameObject.SetActive(false);
                _arrowPointer.gameObject.SetActive(false);
                return;
            }
            // ���� ������� Transform ��������� ��� �� ��������, �������� ��������� �������� ������
            if (target == null)
            {
                int index = Random.Range(0, childCount);
                target = containermonitor.CurrentContainer.transform.GetChild(index);
            }
        }

        // ���� ������� Transform ����������, ������� forward ������� �� ����
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
