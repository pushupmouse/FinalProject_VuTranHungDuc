using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    private const float _DIMINISH_CONST = 100f;

    [SerializeField] private GameObject _floatingPoints;

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
            _defense = 30;
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

        GameObject points = Instantiate(_floatingPoints, transform.position, Quaternion.identity);
        int roundedDamage = Mathf.CeilToInt(damage);
        points.transform.GetChild(0).GetComponent<TextMesh>().text = roundedDamage.ToString();

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
