using UnityEngine;

public static class IntersectionUtil
{
    /// <summary>
    /// Пытается найти точку пересечения двух отрезков [a,b] и [c,d].
    /// </summary>
    /// <param name="a">Начало первого отрезка.</param>
    /// <param name="b">Конец первого отрезка.</param>
    /// <param name="c">Начало второго отрезка.</param>
    /// <param name="d">Конец второго отрезка.</param>
    /// <param name="intersection">Если пересечение найдено, содержит точку пересечения.</param>
    /// <returns>true, если отрезки пересекаются (с учётом их конечных точек), иначе false.</returns>
    public static bool TryGetIntersection(Vector2 a, Vector2 b, Vector2 c, Vector2 d, out Vector2 intersection)
    {
        intersection = Vector2.zero;

        // Направляющие векторы отрезков
        Vector2 r = b - a;
        Vector2 s = d - c;

        // Определитель (кросс-произведение в 2D)
        float rxs = r.x * s.y - r.y * s.x;

        // Если rxs == 0, отрезки параллельны или коллинеарны
        if (Mathf.Approximately(rxs, 0f))
            return false;

        // Вектор от начала первого отрезка до начала второго
        Vector2 c_a = c - a;

        // Параметры t и u для параметрических уравнений отрезков
        float t = (c_a.x * s.y - c_a.y * s.x) / rxs;
        float u = (c_a.x * r.y - c_a.y * r.x) / rxs;

        // Если 0 <= t <= 1 и 0 <= u <= 1, то отрезки пересекаются
        if (t >= 0f && t <= 1f && u >= 0f && u <= 1f)
        {
            intersection = a + t * r;
            return true;
        }

        return false;
    }
}