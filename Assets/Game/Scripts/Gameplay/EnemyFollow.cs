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
    private bool _isRunning;

    #region ANIM
    [SerializeField] private Transform _spriteTransform;

    [SerializeField] private Animator _animator;
    [SerializeField] private float _lockedTime = 0.25f;

    [HideInInspector] private bool _isAttacking = false;
    [HideInInspector] public float AttackDuration = 1f;

    private static readonly int IdleAnim = Animator.StringToHash("Skeleton_Idle");
    private static readonly int RunAnim = Animator.StringToHash("Skeleton_Run");
    private static readonly int AttackAnim = Animator.StringToHash("Skeleton_Attack");

    private int _currentState;
    private float _attackEndTime = 0f;

    private void Update()
    {
        int state = GetState();

        if (state == _currentState)
        {
            return;
        }

        _animator.CrossFade(state, 0f, 0);
        _currentState = state;
    }

    private int GetState()
    {
        if (Time.time < _lockedTime)
        {
            return _currentState;
        }

        if (_isAttacking)
        {
            return LockState(AttackAnim, AttackDuration);
        }

        if (_isRunning)
        {
            return RunAnim;
        }

        return IdleAnim;
    }

    int LockState(int state, float time)
    {
        _lockedTime = Time.time + time;
        return state;
    }
    #endregion

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

            _rb.velocity = direction * _moveSpeed;

            _isRunning = true;

            if (Vector3.Distance(transform.position, _target.position) <= _attackRange)
            {
                _targetInAttackRange = true;

                _rb.velocity = Vector2.zero;

                _isRunning = false;

               //Attack();

                _isAttacking = true;

                _attackEndTime = Time.time + AttackDuration;
            }

            if (_isAttacking && Time.time >= _attackEndTime)
            {
                _isAttacking = false;
            }
        }
        else if (!_targetInDetectionRange || (_targetInAttackRange && Vector2.Distance(transform.position, _target.position) > _attackRange))
        {
            _targetInAttackRange = false;
        }

        if (_isRunning)
        {
            Flip();
        }
    }

    private void Flip()
    {
        Vector2 direction = (_target.position - transform.position).normalized;

        if (direction.x > 0.1f)
        {
            _spriteTransform.localScale = new Vector3(Mathf.Abs(_spriteTransform.localScale.x), _spriteTransform.localScale.y, _spriteTransform.localScale.z);
        }
        else if (direction.x < -0.1f)
        {
            _spriteTransform.localScale = new Vector3(-Mathf.Abs(_spriteTransform.localScale.x), _spriteTransform.localScale.y, _spriteTransform.localScale.z);
        }
    }
}
