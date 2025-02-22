using UnityEngine;

public static class IntersectionUtil
{
    /// <summary>
    /// �������� ����� ����� ����������� ���� �������� [a,b] � [c,d].
    /// </summary>
    /// <param name="a">������ ������� �������.</param>
    /// <param name="b">����� ������� �������.</param>
    /// <param name="c">������ ������� �������.</param>
    /// <param name="d">����� ������� �������.</param>
    /// <param name="intersection">���� ����������� �������, �������� ����� �����������.</param>
    /// <returns>true, ���� ������� ������������ (� ������ �� �������� �����), ����� false.</returns>
    public static bool TryGetIntersection(Vector2 a, Vector2 b, Vector2 c, Vector2 d, out Vector2 intersection)
    {
        intersection = Vector2.zero;

        // ������������ ������� ��������
        Vector2 r = b - a;
        Vector2 s = d - c;

        // ������������ (�����-������������ � 2D)
        float rxs = r.x * s.y - r.y * s.x;

        // ���� rxs == 0, ������� ����������� ��� �����������
        if (Mathf.Approximately(rxs, 0f))
            return false;

        // ������ �� ������ ������� ������� �� ������ �������
        Vector2 c_a = c - a;

        // ��������� t � u ��� ��������������� ��������� ��������
        float t = (c_a.x * s.y - c_a.y * s.x) / rxs;
        float u = (c_a.x * r.y - c_a.y * r.x) / rxs;

        // ���� 0 <= t <= 1 � 0 <= u <= 1, �� ������� ������������
        if (t >= 0f && t <= 1f && u >= 0f && u <= 1f)
        {
            intersection = a + t * r;
            return true;
        }

        return false;
    }
}