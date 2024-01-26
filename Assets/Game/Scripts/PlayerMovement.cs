using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _animator;

    private Vector2 _moveDirection;
    private string _currentAnimationName;

    private void Update()
    {
        GetInputs();
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(_moveDirection.x) > 0.1f || Mathf.Abs(_moveDirection.y) > 0.1f)
        {
            Run();
            return;
        }

        Idle();
    }

    private void GetInputs()
    {
        _moveDirection.x = Input.GetAxisRaw("Horizontal");
        _moveDirection.y = Input.GetAxisRaw("Vertical");
        _moveDirection = _moveDirection.normalized;
    }

    private void Idle()
    {
        PlayAnimation("Idle");
    }

    private void Run()
    {
        _rb.MovePosition(_rb.position + _moveDirection * _moveSpeed * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Euler(new Vector3(0, IsFacingForward() ? 0 : 180, 0));
        PlayAnimation("Run");

        //TODO: FIX ANIMATION WHEN RUNNING VERTICALLY
    }

    private bool IsFacingForward()
    {
        if(_moveDirection.x > 0.1f)
        {
            return true;
        }
        
        return false;
    }

    private void PlayAnimation(string animationName)
    {
        if (_currentAnimationName != animationName)
        {
            _animator.ResetTrigger(animationName);

            _currentAnimationName = animationName;

            _animator.SetTrigger(_currentAnimationName);
        }
    }
}
