using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Image _image;
    HealthController _health;
    void Start()
    {
        _image = GetComponent<Image>();
        _health = PlayerManager.CharacterStatic.health;
        SetBar();
        _health.onHealthChanged += (v) => SetBar();    
    }
    void SetBar()
    {
        _image.fillAmount = _health.CurrentHealth / _health.MaxHealth;
    }
}
