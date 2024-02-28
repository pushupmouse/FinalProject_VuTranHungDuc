using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float knockbackDuration = 0.2f;

    public void ApplyKnockback(Vector3 attackerPos)
    {
        Vector3 knockbackDirection = (transform.position - attackerPos).normalized;

        _rb.velocity = Vector3.zero;
        _rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        StartCoroutine(KnockbackDuration());
    }

    private IEnumerator KnockbackDuration()
    {
        yield return new WaitForSeconds(knockbackDuration);

        _rb.velocity = Vector3.zero;
    }
}
