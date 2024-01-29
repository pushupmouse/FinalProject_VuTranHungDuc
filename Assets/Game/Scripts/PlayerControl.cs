using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _animator;

    private float _moveX, _moveY;
    private Vector2 _moveDirection;
    private string _currentAnimationName;

    private void Update()
    {
        GetInputs();
        ProcessInputs();
    }

    private void GetInputs()
    {
        _moveX = Input.GetAxisRaw(Const.MovementControl.HORIZONTAL);
        _moveY = Input.GetAxisRaw(Const.MovementControl.VERTICAL);

        _moveDirection = new Vector2(_moveX, _moveY).normalized;
    }

    private void ProcessInputs()
    {
        if (Mathf.Abs(_moveX) > 0.1f || Mathf.Abs(_moveY) > 0.1f)
        {
            Run();
        }
        else
        {
            Idle();
        }
    }

    private void Idle()
    {
        SetTriggerAnimation(Const.Animation.IDLE);
    }

    private void Run()
    {
        _rb.MovePosition(_rb.position + _moveDirection * _moveSpeed * Time.fixedDeltaTime);
        SetTriggerAnimation(Const.Animation.RUN);

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

    private void SetTriggerAnimation(string animationName)
    {
        if (_currentAnimationName != animationName)
        {
            _animator.ResetTrigger(animationName);

            _currentAnimationName = animationName;

            _animator.SetTrigger(_currentAnimationName);
        }
    }
}
