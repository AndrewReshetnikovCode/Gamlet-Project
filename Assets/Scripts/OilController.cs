using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;


public class OilController : MonoBehaviour
{
    public LayerMask characterLayer;
    public LayerMask projLayer;
    public List<GameObject> sprites;
    [SerializeField] IgnitionUpgrade _ignition;
    [SerializeField] List<CharacterFacade> _charsInZone = new();
    [SerializeField] float _tickTime = .1f;
    [SerializeField] float _damage = 10f;
    [SerializeField] float _selfDestroyDelay = 130f;
    [SerializeField] float _fireTime = 10f;

    float _lastTick;
    bool _isFired = false;

    public float Size { get => transform.localScale.x; set => transform.localScale = new Vector3(value, transform.localScale.y, value); }
    BoxCollider _collider;
    private void Start()
    {
        _collider = GetComponent<BoxCollider>();
        Invoke(nameof(SelfDestroy), _selfDestroyDelay);
    }
    void SelfDestroy()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (LayerUtil.IsLayerInMask(other.gameObject.layer, projLayer))
        {
            FireUp();
        }
        else if(LayerUtil.IsLayerInMask(other.gameObject.layer, characterLayer))
        {
            CharacterFacade c = other.GetComponentInParent<CharacterFacade>();
            _charsInZone.Add(c);
            CharacterEntersOil(c);
        }
    }
    private void Update()
    {
        if (Time.time - _lastTick > _tickTime)
        {
            _lastTick = Time.time;
            Tick();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (LayerUtil.IsLayerInMask(other.gameObject.layer,characterLayer))
        {
            _charsInZone.Remove(other.GetComponentInParent<CharacterFacade>());

        }
    }
    [ContextMenu(nameof(FireUp))]
    public void FireUp()
    {
        if (_isFired)
        {
            return;
        }
        _isFired = true;
        foreach (GameObject s in sprites)
        {
            s.SetActive(true);
        }
        foreach (var item in _charsInZone)
        {
            item.upgrade.AddUpgrade(_ignition);
        }
        Invoke(nameof(SelfDestroy), _fireTime);
    }
    public void OnNavAgentEnter(object o)
    {
        MonoBehaviour mb = o as MonoBehaviour;
        if (mb)
        {
            CharacterFacade character;
            if(mb.TryGetComponent<CharacterFacade>(out character))
            {
                _charsInZone.Add(character);
                CharacterEntersOil(character);
            }
        } 
    }
    public void OnNavAgentExit(object o)
    {
        MonoBehaviour mb = o as MonoBehaviour;
        if (mb)
        {
            CharacterFacade character;
            if (mb.TryGetComponent<CharacterFacade>(out character))
            {
                _charsInZone.Remove(character);
            }
        }
    }
    void CharacterEntersOil(CharacterFacade c)
    {
        if (_isFired)
        {
            c.upgrade.AddUpgrade(_ignition);
        }
    }
    void Tick()
    {
        if (_isFired)
        {
            foreach (var item in GetOtherOilPuddles())
            {
                Debug.Log($"попал в {item.name}");
                item.GetComponent<OilController>().FireUp();
            }
            List<CharacterFacade> invalidChars = new(_charsInZone.Count);
            foreach (var item in _charsInZone)
            {
                if (item == null)
                {
                    invalidChars.Add(item);
                }
                else
                {
                    item.ApplyDamage(_damage, this);

                }
            }
            foreach (var item in invalidChars)
            {
                _charsInZone.Remove(item);
            }

        }
    }
    Collider[] GetOtherOilPuddles()
    {
        Vector3 center = transform.TransformPoint(_collider.center);
        Vector3 size = transform.TransformVector(_collider.size) / 2;
        size.y *= 10;
        return Physics.OverlapBox(center, size, transform.rotation, LayerMask.GetMask("Oil"));
    }
}
