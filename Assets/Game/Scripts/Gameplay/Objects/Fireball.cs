using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _speed = 500;
    [SerializeField] private float _damage = 0.05f;

    private void FixedUpdate()
    {
        _rb.velocity = transform.right * _speed * Time.fixedDeltaTime;
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            HealthController target = Cache<HealthController>.GetComponent(collision);

            target.TakeFireballDamage(_damage);
        }
    }
}
