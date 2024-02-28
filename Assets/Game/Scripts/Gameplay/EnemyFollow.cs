using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _detectionRange = 10f;
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private float _moveSpeed = 3f;

    private bool _targetInDetectionRange = false;
    private bool _targetInAttackRange = false;

    private void Update()
    {
        if (_target != null && Vector3.Distance(transform.position, _target.position) <= _detectionRange)
        {
            _targetInDetectionRange = true;
        }
        else
        {
            _targetInDetectionRange = false;
        }

        if (_targetInDetectionRange && !_targetInAttackRange)
        {
            Vector3 direction = (_target.position - transform.position).normalized;
            transform.Translate(direction * _moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _target.position) <= _attackRange)
            {
                _targetInAttackRange = true;
            }
        }
        else if (_targetInAttackRange && Vector3.Distance(transform.position, _target.position) > _attackRange)
        {
            _targetInAttackRange = false;
        }
    }
}
