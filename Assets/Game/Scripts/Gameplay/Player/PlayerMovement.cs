using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(InputMovementController))]
[RequireComponent(typeof(InputAttackController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _dashSpeed = 15f;
    [SerializeField] private float _dashTime = 0.2f;
    [SerializeField] private float _dashCooldown = 1f;

    private InputMovementController _movementController;
    private InputAttackController _attackController;
    private bool _isFacingRight = true;

    private void Awake()
    {
        _movementController = GetComponent<InputMovementController>();
        _attackController = GetComponent<InputAttackController>();
    }

    private void FixedUpdate()
    {
        if (_movementController.IsDashing)
        {
            return;
        }

        if (_attackController.IsAttacking)
        {
            _rb.velocity = Vector2.zero;
            return;
        } 

        _rb.velocity = _movementController.MoveDirection * _moveSpeed;

        Flip();
    }

    private void Flip()
    {
        if (_isFacingRight && _movementController.MoveX < 0f || !_isFacingRight && _movementController.MoveX > 0f)
        {
            Vector3 localScale = transform.localScale;

            _isFacingRight = !_isFacingRight;

            localScale.x *= -1f;

            transform.localScale = localScale;
        }
    }

    public IEnumerator Dash()
    {
        _movementController.CanDash = false;
        _movementController.IsDashing = true;

        if(_movementController.MoveDirection != Vector2.zero)
        {
            _rb.velocity = _movementController.MoveDirection * _dashSpeed;
        }
        else
        {
            if (_isFacingRight)
            {
                _rb.velocity = Vector2.right * _dashSpeed;
            }
            else
            {
                _rb.velocity = Vector2.left * _dashSpeed;
            }
        }



        yield return new WaitForSeconds(_dashTime);

        _movementController.IsDashing = false;

        _rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(_dashCooldown);

        _movementController.CanDash = true;
    }
}
