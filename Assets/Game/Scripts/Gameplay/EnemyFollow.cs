using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Transform _target;
    [SerializeField] private float _detectionRange = 10f;
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private float _moveSpeed = 3f;

    private bool _targetInDetectionRange = false;
    private bool _targetInAttackRange = false;

    private void FixedUpdate()
    {
        if (_target != null && Vector2.Distance(transform.position, _target.position) <= _detectionRange)
        {
            _targetInDetectionRange = true;
        }
        else
        {
            _targetInDetectionRange = false;
        }

        if (_targetInDetectionRange && !_targetInAttackRange)
        {
            Vector2 direction = (_target.position - transform.position).normalized;
            _rb.MovePosition(_rb.position + direction * _moveSpeed * Time.fixedDeltaTime);

            if (Vector3.Distance(transform.position, _target.position) <= _attackRange)
            {
                _targetInAttackRange = true;
            }
        }
        else if (!_targetInDetectionRange || (_targetInAttackRange && Vector2.Distance(transform.position, _target.position) > _attackRange))
        {
            _targetInAttackRange = false;
        }
    }
}
