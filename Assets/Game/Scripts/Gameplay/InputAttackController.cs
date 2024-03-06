using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAttack))]
public class InputAttackController : MonoBehaviour
{
    [SerializeField] private float _resetAttackTime = 1f;

    [HideInInspector] public int CurrentAttack = 0;
    [HideInInspector] public bool IsAttacking = false;
    [HideInInspector] public float AttackDuration = 0.25f;

    private PlayerAttack _playerAttack;

    private float _attackEndTime = 0f;
    private float _lastAttackTime;

    private void Awake()
    {
        _playerAttack = GetComponent<PlayerAttack>();
    }

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

            _playerAttack.Attack();

            _lastAttackTime = Time.time;

            IsAttacking = true;

            _attackEndTime = Time.time + AttackDuration;
        }

        if (IsAttacking && Time.time >= _attackEndTime)
        {
            IsAttacking = false;
        }
    }
}