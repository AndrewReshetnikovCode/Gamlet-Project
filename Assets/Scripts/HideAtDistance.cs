using UnityEngine;

public class HideAtDistance : MonoBehaviour
{
    [SerializeField] private float activationDistance = 5f; // Расстояние, на котором объект включается
    private GameObject player; // Ссылка на игрока

    void Start()
    {
        // Находим игрока через его имя
        player = GameObject.Find("Player");

        if (player == null)
        {
            Debug.LogError("Игрок не найден!");
        }
    }

    void Update()
    {
        if (player != null)
        {
            // Рассчитываем расстояние между объектом и игроком
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            // Включаем или выключаем объект в зависимости от расстояния
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
