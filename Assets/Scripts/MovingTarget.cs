using System.Collections;
using UnityEngine;

public class MovingTarget : MonoBehaviour
{
    public Transform directionReference; // Ссылка на объект, задающий направление
    public float speed = 2f; // Скорость движения
    public float distance = 5f; // Расстояние, на которое движется мишень
    public float edgePauseTime = 1f; // Пауза на краю перед сменой направления

    private Vector3 startPoint; // Начальная точка движения
    private Vector3 targetPoint; // Точка назначения
    private Vector3 movementDirection; // Текущее направление движения
    private bool movingToTarget = true; // Флаг направления движения
    private bool isPaused = false; // Флаг паузы

    private void Start()
    {
        if (directionReference == null)
        {
            Debug.LogError("Direction Reference is not assigned.");
            return;
        }

        // Инициализация начальной точки и направления
        startPoint = transform.position;
        movementDirection = directionReference.forward.normalized;
        targetPoint = startPoint + movementDirection * distance;
    }

    private void Update()
    {
        if (isPaused || directionReference == null)
            return;

        // Вычисляем текущую цель
        Vector3 currentTarget = movingToTarget ? targetPoint : startPoint;
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);

        // Проверяем, достигли ли цели
        if (Vector3.Distance(transform.position, currentTarget) < 0.01f)
        {
            StartCoroutine(PauseAndReverseDirection());
        }
    }

    private IEnumerator PauseAndReverseDirection()
    {
        isPaused = true;
        yield return new WaitForSeconds(edgePauseTime);
        movingToTarget = !movingToTarget; // Смена направления

        isPaused = false;
    }
}
