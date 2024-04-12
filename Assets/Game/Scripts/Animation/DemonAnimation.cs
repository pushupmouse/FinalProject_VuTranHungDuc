using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonAnimation : AnimationHandler
{
    #region HASH
    private static readonly int IdleAnim = Animator.StringToHash("Idle");
    private static readonly int RunAnim = Animator.StringToHash("Run");
    private static readonly int AttackAnim = Animator.StringToHash("Attack");
    private static readonly int TakeDamageAnim = Animator.StringToHash("TakeDamage");
    private static readonly int DeathAnim = Animator.StringToHash("Death");
    #endregion

    [SerializeField] private Animator _animator;

    private Enemy _enemy;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }

    protected override void Update()
    {
        base.Update();

        _animator.CrossFade(_currentState, 0f, 0);
    }

    protected override int GetState()
    {
        if (_enemy.IsDead)
        {
            return DeathAnim;
        }

        if (_enemy.IsTakeDamage)
        {
            return TakeDamageAnim;
        }

        if (Time.time < _lockedTime)
        {
            return _currentState;
        }

        if (_enemy.IsAttacking)
        {
            return LockState(AttackAnim, _enemy.AttackDuration);
        }

        if (_enemy.IsRunning)
        {
            return RunAnim;
        }

        return IdleAnim;
    }
}
