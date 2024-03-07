using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange = 0.75f;
    [SerializeField] private LayerMask _enemyLayers;

    [SerializeField] private float _damage = 40f;
    [SerializeField] private float _delay = 0.15f;
    
    public void Attack()
    {
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _enemyLayers);

        foreach (Collider2D enemy in hitTargets)
        {
            if (enemy != null)
            {
                //TODO: cache
                Health enemyHealth = enemy.GetComponent<Health>();
                if (enemyHealth != null)
                {
                    StartCoroutine(DelayedDamage(enemyHealth, _damage, _delay));
                }

                Knockback enemyKnockback = enemy.GetComponent<Knockback>();
                if (enemyKnockback != null)
                {
                    StartCoroutine(DelayedKnockback(enemyKnockback, transform.position, _delay));
                }
            }
        }
    }

    private IEnumerator DelayedDamage(Health enemyHealth, float damage, float delay)
    {
        yield return new WaitForSeconds(delay);
        enemyHealth.TakeDamage(damage);
    }

    private IEnumerator DelayedKnockback(Knockback enemyKnockback, Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay);
        enemyKnockback.ApplyKnockback(position);
    }
}
