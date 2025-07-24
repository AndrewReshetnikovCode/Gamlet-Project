using UnityEngine;

public enum VisualEffects
{
    Fire,
    Electricity
}
public enum ParticleType
{
    Blood
}
public class VisualEffectsController : MonoBehaviour
{
    //Нужно для игрока или тех случаев, когда нам нужно перестать отображать эффекты
    public bool displayEffects;

    [SerializeField] GameObject _fireSprite;
    [SerializeField] GameObject _electroSprite;

    [SerializeField] GameObject _bloodParticlePrefab;
    public void SetActive(VisualEffects effectId, bool value)
    {
        if (displayEffects == false)
        {
            return;
        }

        switch (effectId)
        {
            case VisualEffects.Fire:
                _fireSprite.SetActive(value);
                break;
            case VisualEffects.Electricity:
                _fireSprite.SetActive(value);
                break;
        }

    }
    public void CreateParticle(Vector3 worldCords, Vector3 dir, float intencity, ParticleType particle)
    {
        if (displayEffects == false)
        {
            return;
        }
        GameObject ps = null;
        switch (particle)
        {
            case ParticleType.Blood:
                ps = Instantiate(_bloodParticlePrefab, transform);
                break;
            default:
                break;
        }
        if (ps != null)
        {
            ps.transform.position = worldCords;
            ps.transform.forward = dir;
            //ps.transform.localScale = Vector3.one * intencity;
            ps.GetComponent<ParticleSystem>().Play();
        }
    }
}

