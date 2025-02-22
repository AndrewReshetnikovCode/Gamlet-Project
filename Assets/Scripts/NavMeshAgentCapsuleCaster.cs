using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class NavMeshAgentCapsuleCaster : MonoBehaviour
{
    private CapsuleCollider capsuleCollider;
    private Vector3 capsuleTopOffset;
    private Vector3 capsuleBottomOffset;
    private float capsuleRadius;

    List<Collider> collided = new();
    List<Collider> prevCollided = new();
    void Start()
    {
        // Получаем капсульный коллайдер
        capsuleCollider = GetComponent<CapsuleCollider>();

        // Рассчитываем реальные параметры капсулы с учетом масштаба объекта
        capsuleRadius = capsuleCollider.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.z); // Радиус учитывает масштаб X и Z
        float scaledHeight = Mathf.Max(capsuleCollider.height * transform.lossyScale.y - 2 * capsuleRadius, 0f); // Учитываем масштаб Y

        // Вычисляем оффсеты для верхней и нижней точки
        capsuleTopOffset = Vector3.up * (scaledHeight / 2f);
        capsuleBottomOffset = -capsuleTopOffset;
    }

    void Update()
    {
        CastCapsule();
    }
    private void OnDestroy()
    {
        Debug.Log("qqqq");
    }
    private void CastCapsule()
    {
        collided.Clear();
        // Вычисляем мировые позиции верхней и нижней точки капсулы
        Vector3 worldTop = transform.position + transform.rotation * (capsuleCollider.center + capsuleTopOffset);
        Vector3 worldBottom = transform.position + transform.rotation * (capsuleCollider.center + capsuleBottomOffset);

        // Выполняем капсульный рейкаст
        Collider[] hits = Physics.OverlapCapsule(worldTop, worldBottom, capsuleRadius);

        foreach (Collider hit in hits)
        {
            if (hit.gameObject != gameObject) // Игнорируем самого себя
            {
                if (prevCollided.Contains(hit) == false)
                {
                    hit.SendMessage("OnNavAgentEnter", this, SendMessageOptions.DontRequireReceiver);
                }
                collided.Add(hit);
            }
        }
        foreach (var item in prevCollided)
        {
            if (hits.Contains(item) == false && item != null)
            {
                Debug.Log($"name {name} \ncoords {transform.position.ToString()}");
                item.SendMessage("OnNavAgentExit", this, SendMessageOptions.DontRequireReceiver);
            }
        }
        prevCollided.Clear();
        prevCollided.AddRange(collided);
    }
}
