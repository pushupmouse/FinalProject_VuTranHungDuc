using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputMovementController))]
[RequireComponent(typeof(InputAttackController))]
public class PlayerAnimation : AnimationHandler
{
    #region HASH
    private static readonly int IdleAnim = Animator.StringToHash("Player_Idle");
    private static readonly int RunAnim = Animator.StringToHash("Player_Run");
    private static readonly int DashAnim = Animator.StringToHash("Player_Dash");
    private static readonly int Attack1Anim = Animator.StringToHash("Player_Attack1");
    private static readonly int Attack2Anim = Animator.StringToHash("Player_Attack2");
    private static readonly int Attack3Anim = Animator.StringToHash("Player_Attack3");
    #endregion

    [SerializeField] private Animator _animator;
    
    private InputMovementController _movementController;
    private InputAttackController _attackController;

    private void Awake()
    {
        _movementController = GetComponent<InputMovementController>();
        _attackController = GetComponent<InputAttackController>();
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

        if (_movementController.IsDashing)
        {
            return DashAnim;
        }

        if (_attackController.IsAttacking)
        {
            switch (_attackController.CurrentAttack)
            {
                case 1:
                    return LockState(Attack1Anim, _attackController.AttackDuration);
                case 2:
                    return LockState(Attack2Anim, _attackController.AttackDuration);
                case 3:
                    return LockState(Attack3Anim, _attackController.AttackDuration);
            }
        }

        if (_movementController.MoveX != 0 || _movementController.MoveY != 0)
        {
            return RunAnim;
        }

        return IdleAnim;
    }
}
