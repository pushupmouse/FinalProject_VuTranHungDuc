using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _knockbackForce = 5f;
    [SerializeField] private float _knockbackDuration = 0.2f;

    [HideInInspector] public bool IsKnockbacked;

    public void ApplyKnockback(Vector3 attackerPos)
    {
        Vector3 knockbackDirection = (transform.position - attackerPos).normalized;

        _rb.velocity = Vector3.zero;
        _rb.AddForce(knockbackDirection * _knockbackForce, ForceMode2D.Impulse);
        StartCoroutine(KnockbackDuration());
    }

    private IEnumerator KnockbackDuration()
    {
        IsKnockbacked = true;

        yield return new WaitForSeconds(_knockbackDuration);

        IsKnockbacked = false;

        _rb.velocity = Vector3.zero;
    }
}
