using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HealthController : MonoBehaviour
{
    private const float _DIMINISH_CONST = 100f;

    [SerializeField] private FloatingPointHandler _floatingPoint;
    [SerializeField] private float _maxRecoveryChance = 0.75f;

    private float _maxHealth;
    private float _health;
    private float _defense;
    private float _damageReduction;
    private float _recoveryChance;
    private float _recoveryAmount;

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

        _maxHealth = _health;
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

    public void InitializeRecoveryChance(float value)
    {
        if (value <= 0)
        {
            _recoveryChance = 0;
        }
        else
        {
            _recoveryChance = value;
        }

        _recoveryAmount = _defense * 0.05f + 1;


        if (_recoveryChance > _maxRecoveryChance)
        {
            _recoveryAmount += _recoveryChance - _maxRecoveryChance;
        }
    }

    public void TakeDamage(float amount, bool isCritical)
    {
        float damage = amount * (1 - _damageReduction);
        _health -= damage;

        FloatingPointHandler point = Instantiate(_floatingPoint, transform.position, Quaternion.identity);
        point.DisplayDamageText(Mathf.CeilToInt(damage), isCritical);

        if (_health <= 0)
        {
            Die();
        }
        else
        {
            OnTakeDamage?.Invoke();

            if(Random.value <= _recoveryChance)
            {
                StopCoroutine("HealOverTime");
                StartCoroutine(HealOverTime(_recoveryAmount, 1f));
            }
        }
    }

    private IEnumerator HealOverTime(float amount, float duration)
    {
        float healPerSecond = amount / duration;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            yield return new WaitForSeconds(0.25f);
            _health += healPerSecond;
            FloatingPointHandler point = Instantiate(_floatingPoint, transform.position, Quaternion.identity);
            point.DisplayHealText(Mathf.CeilToInt(healPerSecond));
            _health = Mathf.Min(_health, _maxHealth);
            elapsedTime += 0.25f;
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
    }

    
}
