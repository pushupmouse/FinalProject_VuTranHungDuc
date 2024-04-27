using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    [SerializeField] private ChaseState _chaseState;
    [SerializeField] private bool _playerInChaseRange;

    public override State RunCurrentState()
    {
        if (_playerInChaseRange)
        {
            return _chaseState;
        }
        else
        {
            return this;
        }
    }
}
