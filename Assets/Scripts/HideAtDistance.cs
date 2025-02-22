using UnityEngine;

public class HideAtDistance : MonoBehaviour
{
    [SerializeField] private float activationDistance = 5f; // ����������, �� ������� ������ ����������
    private GameObject player; // ������ �� ������

    void Start()
    {
        // ������� ������ ����� ��� ���
        player = GameObject.Find("Player");

        if (player == null)
        {
            Debug.LogError("����� �� ������!");
        }
    }

    void Update()
    {
        if (player != null)
        {
            // ������������ ���������� ����� �������� � �������
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            // �������� ��� ��������� ������ � ����������� �� ����������
            if (distanceToPlayer <= activationDistance)
            {
                    gameObject.GetComponent<SpriteRenderer>().enabled = true;
                
            }
            else
            {
                    gameObject.GetComponent<SpriteRenderer>().enabled = false;
                
            }
        }
    }
}
