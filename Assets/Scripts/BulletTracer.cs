using UnityEngine;

public class BulletTracer : MonoBehaviour
{
    public void CreateTracer(Vector3 start, Vector3 end, ParticleSystem tracerParticles)
    {
        ParticleSystem newTrail = Instantiate(tracerParticles);
        // ������������� ������� ParticleSystem ����� �������
        Vector3 position = (start + end) / 2;
        newTrail.transform.position = position;

        // ����������� ������� ������ ����� �����
        newTrail.transform.LookAt(end);
        float distance = Vector3.Distance(start, end);

        // ����������� Shape ������
        ParticleSystem.ShapeModule shape = newTrail.shape;
        newTrail.transform.localScale = new(.1f, .1f, distance);

        ParticleSystem.Burst burst = newTrail.emission.GetBurst(0);
        burst.count = distance * 4;
        newTrail.emission.SetBurst(0, burst);
        // ��������� �������
        newTrail.Play();

        // ���������� ������ ����� �����������
        Destroy(newTrail.gameObject, newTrail.main.duration);
    }
}
