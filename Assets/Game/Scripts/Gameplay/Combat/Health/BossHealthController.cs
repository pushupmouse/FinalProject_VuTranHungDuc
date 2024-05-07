using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthController : HealthController
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private float _healthThreshold = 0.5f;

    private bool _conditionCleared = false;

    public override void TakeDamage(float amount, bool isCritical)
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
            if (_currentHealth <= _maxHealth * _healthThreshold && !_conditionCleared)
            {
                if(enemy != null)
                {
                    enemy.UseSkill();
                }
                _conditionCleared = true;
            }

            OnTakeDamage?.Invoke();

            if (Random.value <= _recoveryChance)
            {
                StopCoroutine("HealOverTime");
                StartCoroutine(HealOverTime(_recoveryAmount, 1f));
            }
        }
    }
}
