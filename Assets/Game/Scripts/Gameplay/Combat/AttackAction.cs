using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AttackAction : MonoBehaviour
{
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange = 0.75f;
    [SerializeField] private LayerMask _targetLayers;
    [SerializeField] private float _damage = 40f;
    [SerializeField] private float _delay = 0.15f;

    public void Attack()
    {
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _targetLayers);

        foreach (Collider2D hit in hitTargets)
        {
            if (hit != null)
            {
                Transform target = hit.transform;

                Health hitHealth = Cache<Health>.GetComponent(hit);
                if (hitHealth != null)
                {
                    StartCoroutine(DelayedDamage(hitHealth, _damage, _delay, target));
                }

                Knockback hitKnockback = Cache<Knockback>.GetComponent(hit);
                if (hitKnockback != null)
                {
                    StartCoroutine(DelayedKnockback(hitKnockback, transform.position, _delay, target));
                }
            }
        }
    }

    private IEnumerator DelayedDamage(Health hitHealth, float damage, float delay, Transform target)
    {
        yield return new WaitForSeconds(delay);

        if (target != null && Vector2.Distance(transform.position, target.position) <= _attackRange)
        {
            hitHealth.TakeDamage(damage);
        }
    }

    private IEnumerator DelayedKnockback(Knockback hitKnockback, Vector3 position, float delay, Transform target)
    {
        yield return new WaitForSeconds(delay);

        if (target != null && Vector2.Distance(transform.position, target.position) <= _attackRange)
        {
            hitKnockback.ApplyKnockback(position);
        }
    }
}
