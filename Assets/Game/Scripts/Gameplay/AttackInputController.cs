using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInputController : MonoBehaviour
{
    private float _resetAttackTime = 1f;
    private float _lastAttackTime;
    private float _attackEndTime = 0f;

    [HideInInspector] public int CurrentAttack = 0;
    [HideInInspector] public bool IsAttacking = false;
    [HideInInspector] public float AttackDuration = 0.5f;

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

            //Attack(_currentAttack);

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
