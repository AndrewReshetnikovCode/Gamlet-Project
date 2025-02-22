using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaycastCollector : MonoBehaviour
{
    [SerializeField] private float raycastDistance = 4f; // Настраиваемое расстояние для Raycast
    [SerializeField] private int maxItems = 9; // Максимальное количество предметов
    [SerializeField] private TextMeshProUGUI candyText;
    private int itemCount = 0; // Текущий счётчик предметов

    private void Start()
    {
        candyText.text = "0/9";
    }
    void Update()
    {
        // Проверяем нажатие левой кнопки мыши
        if (Input.GetMouseButtonDown(0))
        {
            // Создаём луч от центра объекта вперёд
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            // Выполняем Raycast
            if (Physics.Raycast(ray, out hit, raycastDistance))
            {
                // Проверяем, что луч попал в объект с тегом "Collectible"
                if (hit.collider.CompareTag("Candy"))
                {
                    itemCount++; // Увеличиваем счётчик
                    candyText.text = itemCount + "/9";
                    Destroy(hit.collider.gameObject);
                    // Проверяем, достигнут ли максимум
                    if (itemCount >= maxItems)
                    {
                        TriggerAction(); // Выполняем действие при достижении максимума
                    }
                }
            }
        }
    }

    // Действие, которое будет выполнено, когда счётчик достигнет максимума
    private void TriggerAction()
    {
        SceneManager.LoadScene("Win Scene");
    }

    // Визуализация луча в редакторе (необязательно)
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * raycastDistance);
    }
}
