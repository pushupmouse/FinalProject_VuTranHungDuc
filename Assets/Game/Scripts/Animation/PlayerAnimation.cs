using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
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
    private static readonly int TakeDamageAnim = Animator.StringToHash("Player_TakeDamage");
    private static readonly int DeathAnim = Animator.StringToHash("Player_Death");
    #endregion

    [SerializeField] private Animator _animator;
    
    private PlayerController _playerController;
    private InputAttackController _attackController;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _attackController = GetComponent<InputAttackController>();
    }

    protected override void Update()
    {
        base.Update();

        _animator.CrossFade(_currentState, 0f, 0);
    }

    protected override int GetState()
    {
        if (_playerController.IsDead)
        {
            return DeathAnim;
        }


        if (_playerController.IsDashing)
        {
            return DashAnim;
        }

        if (Time.time < _lockedTime)
        {
            return _currentState;
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

        if (_playerController.IsTakeDamage)
        {
            return TakeDamageAnim;
        }

        if (_playerController.MoveX != 0 || _playerController.MoveY != 0)
        {
            return RunAnim;
        }

        return IdleAnim;
    }
}
