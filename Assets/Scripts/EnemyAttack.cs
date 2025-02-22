using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float damageAmount = 10f; // Количество урона
    [SerializeField] private float attackInterval = 1f; // Интервал между атаками
    [SerializeField] private float attackRange = 2f; // Дистанция атаки
    [SerializeField] private LayerMask playerLayer; // Слой игрока для проверки попадания Raycast

    private GameObject player; // Ссылка на игрока
    private PlayerHealth playerHealth; // Компонент здоровья игрока
    private float nextAttackTime; // Время для следующей атаки

    void Start()
    {
        // Находим игрока и его компонент здоровья
        player = GameObject.Find("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }

        nextAttackTime = Time.time;
    }

    void Update()
    {
        if (player != null && playerHealth != null)
        {
            // Проверяем, находится ли игрок в радиусе атаки
            if (IsPlayerInAttackRange())
            {
                // Наносим урон игроку с интервалом
                if (Time.time >= nextAttackTime)
                {
                    playerHealth.GetDamage(damageAmount);
                    nextAttackTime = Time.time + attackInterval;
                }
            }
        }
    }

    // Метод для проверки, находится ли игрок в радиусе атаки с помощью Raycast
    private bool IsPlayerInAttackRange()
    {
        RaycastHit hit;
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        // Посылаем Raycast в сторону игрока
        if (Physics.Raycast(transform.position, directionToPlayer, out hit, attackRange, playerLayer))
        {
            if (hit.collider.gameObject == player)
            {
                return true; // Игрок в зоне действия врага
            }
        }

        return false; // Игрок не в зоне действия
    }

    // Визуализация луча для атаки в редакторе (необязательно)
    private void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.red;
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            Gizmos.DrawRay(transform.position, directionToPlayer * attackRange);
        }
    }
}
