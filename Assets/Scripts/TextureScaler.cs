using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class TextureScaler : MonoBehaviour
{
    public float textureSize = 1.0f; // ������ �������� � ������ (��� ������ �������� ����)

    void Start()
    {
        // �������� �������� ������� � ��� ��������
        Renderer renderer = GetComponent<Renderer>();
        Material material = renderer.material;

        // ������ ������� � ������� �����������
        Vector3 objectSize = renderer.bounds.size;

        // ������������ ����� �������� Tiling ��� ��������
        Vector2 newTiling = new Vector2(objectSize.x / textureSize, objectSize.z / textureSize);

        // ��������� ��������� �������� Tiling � ���������
        material.mainTextureScale = newTiling;
    }
}
