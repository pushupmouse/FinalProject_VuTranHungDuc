using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackArea = 0.75f;
    [SerializeField] private LayerMask _targetLayers;
    [SerializeField] private float _maxCritChance = 0.75f;
    [SerializeField] private float _critDamageMult = 0.5f;

    private Knockback _knockback;
    private float _damage;
    private float _critChance;
    private float _damageMult;


    private void Awake()
    {
        _knockback = GetComponent<Knockback>();
    }

    public void InitializeDamage(float value)
    {
        if (value <= 0)
        {
            _damage = 1;
        }
        else
        {
            _damage = value;
        }
    }

    public void InitializeCritChance(float value)
    {
        if (value <= 0)
        {
            _critChance = 0.05f;
        }
        else
        {
            _critChance = value;
        }

        if(_critChance > _maxCritChance)
        {
            _critDamageMult += (_critChance - _maxCritChance) * 2;
            _critChance = _maxCritChance;
        }
    }

    public void InitializeDamageMult(float value)
    {
        if (value <= 0)
        {
            _damageMult = 0;
        }
        else
        {
            _damageMult = value;
        }
    }

    public void Attack()
    {
        if (_knockback != null && _knockback.IsKnockbacked)
        {
            return;
        }

        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(_attackPoint.position, _attackArea, _targetLayers);

        foreach (Collider2D hit in hitTargets)
        {
            if (hit != null)
            {
                Transform target = hit.transform;

                HealthController hitHealth = Cache<HealthController>.GetComponent(hit);
                if (hitHealth != null)
                {
                    bool isCritical = Random.value <= _critChance;

                    DealDamage(hitHealth, _damage, target, isCritical);
                }

                Knockback hitKnockback = Cache<Knockback>.GetComponent(hit);
                if (hitKnockback != null)
                {
                    InflictKnockback(hitKnockback, transform.position, target);
                }
            }
        }
    }

    private void DealDamage(HealthController hitHealth, float damage, Transform target, bool isCritical)
    {
        float finalDamage = (1 + _damageMult) * damage;

        if (target != null && Vector2.Distance(transform.position, target.position) <= _attackArea)
        {
            if (isCritical)
            {
                finalDamage = (1 +_critDamageMult) * finalDamage;
            }

            hitHealth.TakeDamage(finalDamage, isCritical);
        }
    }

    private void InflictKnockback(Knockback hitKnockback, Vector3 position, Transform target)
    {

        if (target != null && Vector2.Distance(transform.position, target.position) <= _attackArea)
        {
            hitKnockback.ApplyKnockback(position);
        }
    }

    public void AttackDemo()
    {
        Debug.Log("HIT");
    }
}
