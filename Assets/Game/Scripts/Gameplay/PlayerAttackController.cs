using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{

    [SerializeField] private float _resetAttackTime = 1f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask enemyLayers;

    [HideInInspector] public int CurrentAttack = 0;
    [HideInInspector] public bool IsAttacking = false;
    [HideInInspector] public float AttackDuration = 0.5f;
    
    private float _attackEndTime = 0f;
    private float _lastAttackTime;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && !IsAttacking)
        {
            if (Time.time - _lastAttackTime > _resetAttackTime)
            {
                CurrentAttack = 1;
            }
            else
            {
                CurrentAttack = (CurrentAttack % 3) + 1;
            }

            Attack();

            _lastAttackTime = Time.time;

            IsAttacking = true;

            _attackEndTime = Time.time + AttackDuration;
        }

        if (IsAttacking && Time.time >= _attackEndTime)
        {
            IsAttacking = false;
        }
    }

    private void Attack()
    {
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitTargets)
        {
            if (enemy != null)
            {
                Debug.Log("Hit " + enemy.name);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
