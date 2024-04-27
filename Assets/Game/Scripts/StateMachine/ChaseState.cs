using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    [SerializeField] private AttackState _attackState;
    [SerializeField] private bool _playerInAttackRange;

    public override State RunCurrentState()
    {
        if (_playerInAttackRange)
        {
            return _attackState;
        }
        else
        {
            return this;
        }
    }
}
