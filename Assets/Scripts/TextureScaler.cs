using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class TextureScaler : MonoBehaviour
{
    public float textureSize = 1.0f; // Размер текстуры в метрах (или других единицах мира)

    void Start()
    {
        // Получаем рендерер объекта и его материал
        Renderer renderer = GetComponent<Renderer>();
        Material material = renderer.material;

        // Размер объекта в мировых координатах
        Vector3 objectSize = renderer.bounds.size;

        // Рассчитываем новый параметр Tiling для текстуры
        Vector2 newTiling = new Vector2(objectSize.x / textureSize, objectSize.z / textureSize);

        // Применяем расчетный параметр Tiling к материалу
        material.mainTextureScale = newTiling;
    }
}
