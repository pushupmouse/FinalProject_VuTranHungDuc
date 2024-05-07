using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthController : HealthController
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private float _healthFirstThreshold = 0.66f;
    [SerializeField] private float _healthSecondThreshold = 0.33f;

    private bool _firstConditionCleared = false;
    private bool _secondConditionCleared = false;

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
            if (_currentHealth <= _maxHealth * _healthFirstThreshold && !_firstConditionCleared)
            {
                if(enemy != null)
                {
                    enemy.UseFirstSkill();
                }
                _firstConditionCleared = true;
            }

            if (_currentHealth <= _maxHealth * _healthSecondThreshold && !_secondConditionCleared)
            {
                if (enemy != null)
                {
                    enemy.UseSecondSkill();
                }
                _secondConditionCleared = true;
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
