using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputController))]
[RequireComponent(typeof(AttackInputController))]
public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _lockedTime = 0.5f;

    private PlayerInputController _playerController;
    private AttackInputController _attackController;

    private static readonly int IdleAnim = Animator.StringToHash("Player_Idle");
    private static readonly int RunAnim = Animator.StringToHash("Player_Run");
    private static readonly int Attack1Anim = Animator.StringToHash("Player_Attack1");
    private static readonly int Attack2Anim = Animator.StringToHash("Player_Attack2");
    private static readonly int Attack3Anim = Animator.StringToHash("Player_Attack3");

    private int _currentState;

    private void Awake()
    {
        _playerController = GetComponent<PlayerInputController>();
        _attackController = GetComponent<AttackInputController>();
    }

    private void Update()
    {
        int state = GetState();

        if (state == _currentState)
        {
            return;
        }

        _animator.CrossFade(state, 0, 0);
        _currentState = state;
    }

    private int GetState()
    {
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

        if (_playerController.MoveX != 0 || _playerController.MoveY != 0)
        {
            return RunAnim;
        }

        return IdleAnim;

        int LockState(int state, float time)
        {
            _lockedTime = Time.time + time;
            return state;
        }
    }
}
