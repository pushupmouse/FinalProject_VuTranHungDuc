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
    [SerializeField] private float _hitDuration = 0.2f;
    [SerializeField] private float _attackCooldown = 1f;

    [HideInInspector] public float AttackDuration = 1f;
    [HideInInspector] public bool IsRunning;
    [HideInInspector] public bool IsAttacking = false;
    [HideInInspector] public bool IsTakeDamage;
    [HideInInspector] public bool IsDead;

    private HealthController _health;
    private Knockback _knockback;
    private AttackController _attackAction;
    private bool _targetInDetectionRange = false;
    private float _attackEndTime = 0f;
    private float _lastAttackTime;

    private void Awake()
    {
        _health = GetComponent<HealthController>();
        _knockback = GetComponent<Knockback>();
        _attackAction = GetComponent<AttackController>();
    }

    private void Start()
    {
        _health.OnDeath -= HandleDeath;
        _health.OnDeath += HandleDeath;
        _health.OnTakeDamage -= HandleTakeDamage;
        _health.OnTakeDamage += HandleTakeDamage;
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

            if (Vector3.Distance(transform.position, _target.position) <= _attackRange && Time.time - _lastAttackTime >= _attackCooldown)
            {
                _attackAction.Attack();
                _lastAttackTime = Time.time;
            }
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
            IsAttacking = false;
            return;
        }

        if (_targetInDetectionRange)
        {
            Vector2 direction = (_target.position - transform.position).normalized;

            if (!IsAttacking)
            {
                _rb.velocity = direction * _moveSpeed;
            }
            else
            {
                _rb.velocity = Vector2.zero;
            }

            IsRunning = true;

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

    public void HandleTakeDamage()
    {
        IsTakeDamage = true;
        StartCoroutine(ResetTakeDamage());
    }

    private IEnumerator ResetTakeDamage()
    {
        yield return new WaitForSeconds(_hitDuration);
        IsTakeDamage = false;
    }

    private void HandleDeath()
    {
        IsDead = true;
        gameObject.layer = LayerMask.NameToLayer("Dead");
        Destroy(gameObject, 2f);
    }
}