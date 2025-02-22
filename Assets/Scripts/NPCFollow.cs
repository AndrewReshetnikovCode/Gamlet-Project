using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCFollow : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent navMeshAgent;
    private bool isKnockedback = false;

    [SerializeField] private float knockbackDistance = 2.0f;  // ����������, �� ������� NPC ����� ������������� ����� ��� ��������� �����
    [SerializeField] private float knockbackSpeed = 5.0f;  // �������� ������������

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        player = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        if (!isKnockedback && player != null)
        {
            navMeshAgent.SetDestination(player.position);
        }
    }

    public void ApplyKnockback()
    {
        if (isKnockedback) return;

        Vector3 knockbackDirection = (transform.position - player.position).normalized;
        Vector3 knockbackTarget = transform.position + knockbackDirection * knockbackDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(knockbackTarget, out hit, knockbackDistance, NavMesh.AllAreas))
        {
            // ���� ������� ����� ��������� �� NavMesh, ��������� �������� ������������
            StartCoroutine(KnockbackCoroutine(hit.position));
            StartCoroutine(ChangeColorCoroutine());
        }
    }

    private IEnumerator KnockbackCoroutine(Vector3 targetPosition)
    {
        isKnockedback = true;
        navMeshAgent.isStopped = true;

        float elapsedTime = 0f;
        Vector3 initialPosition = transform.position;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * knockbackSpeed;
            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime);

            yield return null;
        }

        navMeshAgent.isStopped = false;
        isKnockedback = false;
    }

    private IEnumerator ChangeColorCoroutine()
    {
        // ������� ������� � �������� �����
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * 2f;
            GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, elapsedTime);
            yield return null;
        }

        // ������� ������� ������� � ��������� �����
        elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * 2f;
            GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, Color.white, elapsedTime);
            yield return null;
        }
    }
}
