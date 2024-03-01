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
    private bool _canDash = true;
    private bool _isDashing;

    private void Awake()
    {
        _movementController = GetComponent<InputMovementController>();
        _attackController = GetComponent<InputAttackController>();
    }

    private void Update()
    {
        if (_isDashing)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && _canDash)
        {
            StartCoroutine(Dash());
        }

        Flip();
    }

    private void FixedUpdate()
    {
        if (_isDashing)
        {
            return;
        }

        if (_attackController.IsAttacking)
        {
            _rb.velocity = Vector2.zero;
            return;
        } 

        _rb.velocity = _movementController.MoveDirection * _moveSpeed;
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

    private IEnumerator Dash()
    {
        _canDash = false;
        _isDashing = true;

        _rb.velocity = _movementController.MoveDirection * _dashSpeed;

        yield return new WaitForSeconds(_dashTime);

        _isDashing = false;

        _rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(_dashCooldown);

        _canDash = true;
    }
}
