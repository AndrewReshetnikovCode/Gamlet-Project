using UnityEngine;

public class OutlineController : MonoBehaviour
{
    [SerializeField] private float activationDistance = 5f; // Расстояние, на котором включается Outline
    private GameObject player; // Ссылка на игрока
    private Outline outlineComponent; // Ссылка на компонент Outline

    void Start()
    {
        // Находим игрока через его имя
        player = GameObject.Find("Player");

        // Получаем компонент Outline на объекте
        outlineComponent = GetComponent<Outline>();

        if (outlineComponent == null)
        {
            Debug.LogError("Компонент Outline не найден на объекте!");
        }
    }

    void Update()
    {
        if (player != null && outlineComponent != null)
        {
            // Рассчитываем расстояние между объектом и игроком
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            // Включаем или выключаем Outline в зависимости от расстояния
            if (distanceToPlayer <= activationDistance)
            {
                outlineComponent.enabled = true; // Включаем Outline
            }
            else
            {
                outlineComponent.enabled = false; // Выключаем Outline
            }
        }
    }
}
