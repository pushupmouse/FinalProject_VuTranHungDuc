using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    #region CONST
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";
    #endregion

    #region HASHING
    private static readonly int IdleAnim = Animator.StringToHash("Player_Idle");
    private static readonly int RunAnim = Animator.StringToHash("Player_Run");
    #endregion

    [SerializeField] private float _moveSpeed;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _lockedTime = 0.5f;

    private float _moveX, _moveY;
    private Vector2 _moveDirection;
    private int _currentState;

    private int _currentAttack = 0;
    private float _resetAttackTime = 1f;
    private float _lastAttackTime;

    private bool _isAttacking = false;
    private float _attackDuration = 0.5f; // Adjust as needed
    private float _attackEndTime = 0f;

    private void Update()
    {
        GetMoveInputs();

        GetAttackInputs();

        PlayAnimation();
    }

    private void FixedUpdate()
    {
        if (!_isAttacking)
        {
            Move();
        }
    }


    private void GetMoveInputs()
    {
        _moveX = Input.GetAxisRaw(HORIZONTAL);
        _moveY = Input.GetAxisRaw(VERTICAL);

        _moveDirection = new Vector2(_moveX, _moveY).normalized;
    }

    private void Move()
    {
        _rb.MovePosition(_rb.position + _moveDirection * _moveSpeed * Time.fixedDeltaTime);

        //Flip the character
        if (_moveX > 0.1f)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else if (_moveX < -0.1f)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
    }

    private void GetAttackInputs()
    {
        if (Input.GetKeyDown(KeyCode.J) && !_isAttacking)
        {
            if (Time.time - _lastAttackTime > _resetAttackTime)
            {
                _currentAttack = 1;
            }
            else
            {
                _currentAttack = (_currentAttack % 3) + 1;
            }

            Attack(_currentAttack);

            _lastAttackTime = Time.time;

            _isAttacking = true;

            _attackEndTime = Time.time + _attackDuration;
        }

        if (_isAttacking && Time.time >= _attackEndTime)
        {
            _isAttacking = false; // Player can move again after attack duration ends
        }
    }

    private void Attack(int attackNumber)
    {
        Debug.Log("Performing Attack " + attackNumber);
    }

    private void PlayAnimation()
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

        //if attacking lock
        //if dashing lock
        return _moveX == 0 && _moveY == 0 ? IdleAnim : RunAnim;

        //int LockState(int state, float time)
        //{
        //    _lockedTime = Time.time + time;
        //    return state;
        //}
    }
}
