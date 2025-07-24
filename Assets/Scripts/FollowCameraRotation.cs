using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class FollowCameraRotation : MonoBehaviour
{
    [Tooltip("Камера, с которой будет синхронизироваться поворот объекта. Если не задана, используется главная камера.")]
    public Camera targetCamera;

    private RotationConstraint rotationConstraint;

    void Start()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        rotationConstraint = GetComponent<RotationConstraint>();
        if (rotationConstraint == null)
        {
            rotationConstraint = gameObject.AddComponent<RotationConstraint>();
        }

        rotationConstraint.SetSources(new List<ConstraintSource>());

        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = targetCamera.transform;
        source.weight = 1.0f;
        rotationConstraint.AddSource(source);

        // Активируем ограничение и блокируем дальнейшие изменения источников
        rotationConstraint.constraintActive = true;
        rotationConstraint.locked = true;
    }
}
