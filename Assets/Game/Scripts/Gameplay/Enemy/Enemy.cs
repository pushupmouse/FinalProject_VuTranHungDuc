using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Transform _spriteTransform;
    [SerializeField] private Transform _target;
    [SerializeField] private float _detectionRange = 10f;
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private Transform _hpBar;
    [SerializeField] private Knockback _knockback;
    [SerializeField] protected HealthController _health;

    [HideInInspector] public bool IsRunning;
    [HideInInspector] public bool IsAttacking = false;
    [HideInInspector] public bool IsDead;

    public float AttackDuration = 1f;
    private bool _targetInDetectionRange = false;
    private float _attackEndTime = 0f;
    public Action<Enemy> OnEnemyDeath;

    private void Start()
    {
        _health.OnDeath -= HandleDeath;
        _health.OnDeath += HandleDeath;
        IsDead = false;
    }

    private void Update()
    {
        if (_knockback.IsKnockbacked || IsDead)
        {
            return;
        }

        if (_target != null && Vector2.Distance(transform.position, _target.position) <= _detectionRange)
        {
            _targetInDetectionRange = true;
        }
        else
        {
            _targetInDetectionRange = false;
        }

        if (IsRunning)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        if (_knockback.IsKnockbacked || IsDead)
        {
            return;
        }

        if (_targetInDetectionRange)
        {
            if(_target == null)
            {
                return;
            }

            Vector2 direction = (_target.position - transform.position).normalized;

            if (!IsAttacking)
            {
                _rb.velocity = direction * _moveSpeed * Time.fixedDeltaTime;
                IsRunning = true;
            }
            else
            {
                _rb.velocity = Vector2.zero;
            }


            if (Vector3.Distance(transform.position, _target.position) <= _attackRange)
            {
                _rb.velocity = Vector2.zero;
                IsRunning = false;
                
                if (!IsAttacking)
                {
                    IsAttacking = true;
                    _attackEndTime = Time.time + AttackDuration;
                }
            }

            if (IsAttacking && Time.time >= _attackEndTime)
            {
                IsAttacking = false;
            }
        }
        else
        {
            _rb.velocity = Vector2.zero;
            IsRunning = false;
            IsAttacking = false;
        }
    }


    private void Flip()
    {
        if (_target == null)
        {
            return;
        }

        Vector2 direction = (_target.position - transform.position).normalized;

        if (direction.x > 0.1f)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            _hpBar.localScale = new Vector3(Mathf.Abs(_hpBar.localScale.x), _hpBar.localScale.y, _hpBar.localScale.z);
        }
        else if (direction.x < -0.1f)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            _hpBar.localScale = new Vector3(-Mathf.Abs(_hpBar.localScale.x), _hpBar.localScale.y, _hpBar.localScale.z);
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void HandleDeath()
    {
        IsDead = true;
        gameObject.layer = LayerMask.NameToLayer("Dead");
        OnEnemyDeath?.Invoke(this);
        Destroy(gameObject, 2f);
    }

    public virtual void UseFirstSkill()
    {
    }

    public virtual void UseSecondSkill()
    {
    }
}