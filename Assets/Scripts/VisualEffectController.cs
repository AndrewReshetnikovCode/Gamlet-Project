using UnityEngine;


public class VisualEffectController : MonoBehaviour
{
    [SerializeField] GameObject _fireSprite;
    public virtual void ActivateBurn(bool v)
    {
        _fireSprite.SetActive(v);
    }
}

