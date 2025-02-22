using UnityEngine;


public class TrueSightObject : MonoBehaviour
{
    [SerializeField] LayerMask _wallLayer;
    [SerializeField] SpriteRenderer _sr;
    [SerializeField] Renderer _renderer;
    [SerializeField] Material _trueSightMaterial;
    Material _defaultMaterial;
    CharacterFacade character;
    private void Start()
    {
        _defaultMaterial = _renderer.material;
        PlayerManager.Instance.onTrueSightChange += OnTrueSightChange;
        OnTrueSightChange(PlayerManager.Instance.TrueSightActive);
        character = GetComponentInParent<CharacterFacade>();
    }
    private void Update()
    {
        if (character.RangeToPlayer > PlayerManager.Instance.TrueSightRange)
        {
            Off();
            _sr.enabled = false;
        }
        else if(PlayerManager.Instance.TrueSightActive)
        {
            On();
            Transform cam = Camera.main.transform;
            if (Physics.Raycast(cam.position, transform.position - cam.position, (transform.position - cam.position).magnitude, _wallLayer) == true)
            {
                _sr.enabled = true;
                _sr.transform.forward = -cam.forward;
            }
            else
            {
                _sr.enabled = false;
            }
        }

    }
    private void OnDestroy()
    {
        PlayerManager.Instance.onTrueSightChange -= OnTrueSightChange;
    }
    void OnTrueSightChange(bool v)
    {
        if (v == true)
        {
            On();
        }
        else
        {
            Off();
        }
    }
    void On()
    {
        _renderer.material = _trueSightMaterial;
    }
    void Off()
    {
        _renderer.material = _defaultMaterial;
    }
}
