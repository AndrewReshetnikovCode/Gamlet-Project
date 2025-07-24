using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BloodFountainUIParticles : MonoBehaviour
{
    [SerializeField] Image _bar;
    [SerializeField] Vector3 _offset;
    [SerializeField] ParticleSystem _ps;
    private void Start()
    {
        _ps.Stop();
        HealthController health = PlayerManager.CharacterStatic.health;
        health.onDamageApplied += (v, s) => { Spawn(Vector3.up); };
    }
    void Spawn(Vector3 dir)
    {
        ParticleSystem newPs = Instantiate(_ps, _ps.transform.position, _ps.transform.rotation, _ps.transform.parent);
        newPs.AddComponent<SelfDestruct>().ResetTimer(newPs.main.startLifetime.constant);
        Vector3 barRightEdgeWorld = GetFilledBarPoint(_bar);
        newPs.transform.position = barRightEdgeWorld + _offset;
        newPs.transform.forward = Camera.main.transform.right;
        //newPs.transform.up = Camera.main.transform.up;
        newPs.Play();
    }
    public static Vector3 GetFilledBarPoint(Image image)
    {
        if (image == null)
        {
            Debug.LogError("Image is null");
            return Vector3.zero;
        }

        if (image.type != Image.Type.Filled || image.fillMethod != Image.FillMethod.Horizontal)
        {
            Debug.LogError("Image должен иметь тип Filled и метод заполнения Horizontal");
            return Vector3.zero;
        }

        if (image.fillOrigin != 0)
        {
            Debug.LogWarning("Функция рассчитана на fillOrigin == 0 (заполнение слева направо). Для иных значений результат может быть иным.");
        }

        Vector3[] corners = new Vector3[4];
        image.rectTransform.GetWorldCorners(corners);
        // Углы:
        // corners[0] - нижний левый, corners[1] - верхний левый,
        // corners[2] - верхний правый, corners[3] - нижний правый.

        Vector3 leftEdge = (corners[0] + corners[1]) * 0.5f;
        Vector3 rightEdge = (corners[2] + corners[3]) * 0.5f;

        Vector3 imageRightPoint = Vector3.Lerp(leftEdge, rightEdge, image.fillAmount);
        return imageRightPoint;
    }
}