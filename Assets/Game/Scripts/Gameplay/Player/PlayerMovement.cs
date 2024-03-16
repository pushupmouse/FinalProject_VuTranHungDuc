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
    [SerializeField] private float _staminaRegenRate = 10f;
    [SerializeField] private float _staminaConsumption = 50f;
    [SerializeField] private float _dashCooldown = 1f;
    [SerializeField] private StaminaBar _staminaBar;

    private InputMovementController _movementController;
    private InputAttackController _inputAttackController;
    private bool _isFacingRight = true;
    private float _maxStamina = 100f;
    private float _currentStamina;

    private void Awake()
    {
        _movementController = GetComponent<InputMovementController>();
        _inputAttackController = GetComponent<InputAttackController>();
    }

    private void Start()
    {
        _currentStamina = _maxStamina;
    }

    private void Update()
    {
        if (_currentStamina < _maxStamina && !_movementController.IsDashing)
        {
            _currentStamina += _staminaRegenRate * Time.deltaTime;
            _currentStamina = Mathf.Clamp(_currentStamina, 0f, _maxStamina);

            if(_staminaBar != null)
            {
                _staminaBar.SetStamina(_currentStamina);
            }
        }
    }

    private void FixedUpdate()
    {
        if (_movementController.IsDashing)
        {
            return;
        }

        if (_inputAttackController.IsAttacking)
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
        if (_currentStamina >= _staminaConsumption)
        {
            _currentStamina -= _staminaConsumption;
            if (_staminaBar != null)
            {
                _staminaBar.SetStamina(_currentStamina);
            }

            _movementController.CanDash = false;
            _movementController.IsDashing = true;

            if (_movementController.MoveDirection != Vector2.zero)
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
}
