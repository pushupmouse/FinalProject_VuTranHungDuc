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

    private void Update()
    {
        GetInputs();

        PlayAnimation();
    }

    private void FixedUpdate()
    {
        ProcessInputs();
    }

    private void GetInputs()
    {
        _moveX = Input.GetAxisRaw(HORIZONTAL);
        _moveY = Input.GetAxisRaw(VERTICAL);

        _moveDirection = new Vector2(_moveX, _moveY).normalized;
    }

    private void ProcessInputs()
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

        int LockState(int state, float time)
        {
            _lockedTime = Time.time + time;
            return state;
        }
    }
}
