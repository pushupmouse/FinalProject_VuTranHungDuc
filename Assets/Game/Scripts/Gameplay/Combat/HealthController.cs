using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    private const float _DIMINISH_CONST = 100f;

    [SerializeField] private FloatingPointHandler _floatingPoint;

    private float _health;
    private float _defense;
    private float _damageReduction;

    public Action OnTakeDamage;
    public Action OnDeath;

    public void InitializeHealth(float value)
    {
        if (value <= 0)
        {
            _health = 100;
        }
        else
        {
            _health = value;
        }
    }

    public void InitializeDamageRed(float value)
    {
        if (value <= 0)
        {
            _defense = 0;
        }
        else
        {
            _defense = value;
        }

        _damageReduction = (float)Math.Round(_defense / (_defense + _DIMINISH_CONST), 2);
    }

    public void TakeDamage(float amount)
    {
        float damage = amount * (1 - _damageReduction);
        _health -= damage;

        FloatingPointHandler point = Instantiate(_floatingPoint, transform.position, Quaternion.identity);
        point.DisplayDamageText(Mathf.CeilToInt(damage));

        if (_health <= 0)
        {
            Die();
        }
        else
        {
            OnTakeDamage?.Invoke();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
    }

    
}
