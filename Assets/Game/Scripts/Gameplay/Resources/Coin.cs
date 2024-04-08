using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Coin : MonoBehaviour
{
    [SerializeField] public Rigidbody2D _rb;
    [SerializeField] private float _speed = 10f;

    private Transform _target;
    private bool _toCollect = false;

    void FixedUpdate()
    {
        if (!_toCollect)
        {
            return;
        }

        if (_target != null)
        {
            Vector3 direction = (_target.position - transform.position).normalized;

            Vector3 targetVelocity = direction * _speed;

            _rb.velocity = targetVelocity;
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void CollectCoins()
    {
        _toCollect = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            CoinManager.Instance.AddCoins(1);

            Destroy(gameObject);
        }
    }
}
