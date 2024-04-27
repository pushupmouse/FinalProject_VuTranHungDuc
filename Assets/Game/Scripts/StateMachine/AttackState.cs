using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    [SerializeField] private ChaseState _chaseState;

    public override State RunCurrentState()
    {
        Debug.Log("Attack");
        return _chaseState;
    }
}
