using UnityEngine;

public class OutlineController : MonoBehaviour
{
    [SerializeField] private float activationDistance = 5f; // ����������, �� ������� ���������� Outline
    private GameObject player; // ������ �� ������
    private Outline outlineComponent; // ������ �� ��������� Outline

    void Start()
    {
        // ������� ������ ����� ��� ���
        player = GameObject.Find("Player");

        // �������� ��������� Outline �� �������
        outlineComponent = GetComponent<Outline>();

        if (outlineComponent == null)
        {
            Debug.LogError("��������� Outline �� ������ �� �������!");
        }
    }

    void Update()
    {
        if (player != null && outlineComponent != null)
        {
            // ������������ ���������� ����� �������� � �������
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            // �������� ��� ��������� Outline � ����������� �� ����������
            if (distanceToPlayer <= activationDistance)
            {
                outlineComponent.enabled = true; // �������� Outline
            }
            else
            {
                outlineComponent.enabled = false; // ��������� Outline
            }
        }
    }
}
