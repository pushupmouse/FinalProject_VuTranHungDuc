using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnimation : AnimationHandler
{
    [SerializeField] private Animator _animator;

    private EnemyBehaviour _enemyBehavior;

    private static readonly int IdleAnim = Animator.StringToHash("Skeleton_Idle");
    private static readonly int RunAnim = Animator.StringToHash("Skeleton_Run");
    private static readonly int AttackAnim = Animator.StringToHash("Skeleton_Attack");

    private void Awake()
    {
        _enemyBehavior = GetComponent<EnemyBehaviour>();
    }

    protected override void Update()
    {
        base.Update();

        _animator.CrossFade(_currentState, 0f, 0);
    }

    protected override int GetState()
    {
        if (Time.time < _lockedTime)
        {
            return _currentState;
        }

        if (_enemyBehavior.IsAttacking)
        {
            return LockState(AttackAnim, _enemyBehavior.AttackDuration);
        }

        if (_enemyBehavior.IsRunning)
        {
            return RunAnim;
        }

        return IdleAnim;
    }
}
