using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

class DamageInfoManager : MonoBehaviour
{
    [SerializeField] GameObject damageTextPrefab;
    Camera uiCamera;
    [SerializeField] float _maxDistanceToDisplay;
    [SerializeField] float _randomOffsetMin;
    [SerializeField] float _randomOffsetMax;
    [SerializeField] float _repulsionStrength;

    List<RectTransform> _spawnedObjects = new();
    private void Start()
    {
        EventBus.Register<(Vector3, float)>(nameof(OnDamage), OnDamage);
        uiCamera = Camera.main;
    }
    private void Update()
    {
        _spawnedObjects.RemoveAll(item => item == null);
        ApplyRepulsion();
    }
    void OnDamage((Vector3, float) data)
    {
        var damageText = Instantiate(damageTextPrefab, data.Item1 + (Random.onUnitSphere * Random.Range(_randomOffsetMin, _randomOffsetMax)), Quaternion.identity);
        _spawnedObjects.Add(damageText.GetComponentInChildren<TMP_Text>().rectTransform);
        damageText.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = data.Item2.ToString();
        Destroy(damageText, 2.033f);
    }

    bool IsOverlapping(RectTransform rect1, RectTransform rect2)
    {
        Rect r1 = GetWorldRect(rect1);
        Rect r2 = GetWorldRect(rect2);
        return r1.Overlaps(r2);
    }

    Rect GetWorldRect(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        return new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);
    }
    void ApplyRepulsion()
    {
        foreach (RectTransform rect in _spawnedObjects)
        {
            foreach (RectTransform other in _spawnedObjects)
            {
                if (rect != other)
                {
                    //float distance = GetScreenDistance(rect, other);
                    Vector2 screenPos1 = RectTransformUtility.WorldToScreenPoint(uiCamera, rect.position);
                    Vector2 screenPos2 = RectTransformUtility.WorldToScreenPoint(uiCamera, other.position);

                    Vector2 direction = (screenPos1 - screenPos2).normalized;
                    if (direction == Vector2.zero)
                    {
                        direction = Vector2.right;
                    }
                    float force = _repulsionStrength / Vector2.Distance(screenPos1,screenPos2);

                    rect.anchoredPosition += direction * force * Time.deltaTime;
                }
            }
        }
    }
    float GetScreenDistance(RectTransform rect1, RectTransform rect2)
    {
        Vector2 screenPos1 = RectTransformUtility.WorldToScreenPoint(uiCamera, rect1.position);
        Vector2 screenPos2 = RectTransformUtility.WorldToScreenPoint(uiCamera, rect2.position);
        return Vector2.Distance(screenPos1, screenPos2);
    }

}