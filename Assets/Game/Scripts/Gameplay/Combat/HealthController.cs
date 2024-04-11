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
    [SerializeField] public HealthBar _healthBar;
    [SerializeField] private float _tick = 0.25f;

    private float _maxHealth = 0;
    private float _currentHealth;
    private float _defense;
    private float _damageReduction;
    private float _recoveryChance;
    private float _recoveryAmount;
    private bool _healthChanged = false;
    public float MaxHealth => _maxHealth;
    public Action OnTakeDamage;
    public Action OnDeath;

    private void Update()
    {
        if (_healthChanged && _healthBar != null)
        {
            _healthBar.SetHealth(_currentHealth);
            _healthChanged = false;
        }
    }

    private void SetHealthBar()
    {
        if (_healthBar != null)
        {
            _healthBar.SetMaxHealth(_maxHealth);
            _healthBar.SetHealth(_currentHealth);
        }
    }

    public void InitializeHealth(float value)
    {
        float oldMaxHealth;

        if (value <= 0)
        {
            oldMaxHealth = _maxHealth;
            _maxHealth = 100;
        }
        else
        {
            oldMaxHealth = _maxHealth;
            _maxHealth = value;
        }

        _currentHealth += _maxHealth - oldMaxHealth;

        SetHealthBar();
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
        _currentHealth -= damage;
        _healthChanged = true;

        FloatingPointHandler point = Instantiate(_floatingPoint, transform.position, Quaternion.identity);
        point.DisplayDamageText(Mathf.CeilToInt(damage), isCritical);

        if (_currentHealth <= 0)
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

    public IEnumerator HealOverTime(float amount, float duration)
    {
        float healPerTick = amount / duration;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            yield return new WaitForSeconds(_tick);
            _currentHealth += healPerTick;
            _healthChanged = true;
            FloatingPointHandler point = Instantiate(_floatingPoint, transform.position, Quaternion.identity);
            point.DisplayHealText(Mathf.CeilToInt(healPerTick));
            _currentHealth = Mathf.Min(_currentHealth, _maxHealth);
            elapsedTime += _tick;
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
    }
}
