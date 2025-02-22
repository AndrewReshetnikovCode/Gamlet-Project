using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab; // Префаб врага
    [SerializeField] private float spawnInterval = 3f; // Время между появлениями врагов
    [SerializeField] private float minSpawnRadius = 5f; // Минимальный радиус появления
    [SerializeField] private float maxSpawnRadius = 10f; // Максимальный радиус появления
    [SerializeField] private LayerMask groundLayer; // Слой для проверки поверхности
    [SerializeField] private float groundCheckDistance = 10f; // Расстояние для проверки поверхности

    private GameObject player; // Ссылка на игрока

    void Start()
    {
        // Находим игрока через его имя
        player = GameObject.Find("Player");

        if (player != null)
        {
            // Запускаем появление врагов с интервалом
            InvokeRepeating("SpawnEnemy", spawnInterval, spawnInterval);
        }
        else
        {
            Debug.LogError("Игрок не найден!");
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab != null && player != null)
        {
            // Вычисляем случайное расстояние от игрока в пределах заданного радиуса
            float spawnDistance = Random.Range(minSpawnRadius, maxSpawnRadius);

            // Выбираем случайный угол для размещения врага
            float spawnAngle = Random.Range(0f, 360f);
            Vector3 spawnDirection = new Vector3(Mathf.Sin(spawnAngle), 0f, Mathf.Cos(spawnAngle));

            // Вычисляем позицию для спавна врага
            Vector3 spawnPosition = player.transform.position + spawnDirection * spawnDistance;

            // Проверяем наличие поверхности под предполагаемым местом спавна
            if (IsGroundBelow(spawnPosition))
            {
                // Спавним врага, если под ним есть поверхность
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Нет поверхности под точкой спавна, враг не создан.");
            }
        }
        else
        {
            Debug.LogError("Префаб врага не установлен или игрок не найден.");
        }
    }

    // Метод для проверки наличия поверхности под точкой спавна
    private bool IsGroundBelow(Vector3 spawnPosition)
    {
        RaycastHit hit;
        // Посылаем луч вниз от позиции спавна и проверяем, есть ли поверхность
        if (Physics.Raycast(spawnPosition + Vector3.up * groundCheckDistance, Vector3.down, out hit, groundCheckDistance * 2, groundLayer))
        {
            return true; // Поверхность найдена
        }
        return false; // Поверхности нет
    }

    // Визуализация луча в редакторе (необязательно)
    private void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.red;
            Vector3 spawnPosition = player.transform.position;
            Gizmos.DrawRay(spawnPosition + Vector3.up * groundCheckDistance, Vector3.down * groundCheckDistance * 2);
        }
    }
}
