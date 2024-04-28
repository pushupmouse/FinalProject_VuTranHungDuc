using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerController))]
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
    [SerializeField] public StaminaBar _staminaBar;

    private PlayerController _playerController;
    private InputAttackController _inputAttackController;
    private bool _isFacingRight = true;
    private float _maxStamina = 100f;
    private float _currentStamina;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _inputAttackController = GetComponent<InputAttackController>();
    }

    private void Start()
    {
        OnInit();
    }

    private void Update()
    {
        if (_currentStamina < _maxStamina && !_playerController.IsDashing)
        {
            _currentStamina += _staminaRegenRate * Time.deltaTime;
            _currentStamina = Mathf.Clamp(_currentStamina, 0f, _maxStamina);

            if(_staminaBar != null)
            {
                _staminaBar.SetStamina(_currentStamina);
            }
        }
    }

    private void OnInit()
    {
        _currentStamina = _maxStamina;
    }

    private void FixedUpdate()
    {
        if (_playerController.IsDashing)
        {
            return;
        }

        if (_inputAttackController.IsAttacking)
        {
            _rb.velocity = Vector2.zero;
            return;
        } 

        _rb.velocity = _playerController.MoveDirection * _moveSpeed;

        Flip();
    }

    private void Flip()
    {
        if (_isFacingRight && _playerController.MoveX < 0f || !_isFacingRight && _playerController.MoveX > 0f)
        {
            Vector3 localScale = transform.localScale;

            _isFacingRight = !_isFacingRight;

            localScale.x *= -1f;

            transform.localScale = localScale;
        }
    }

    public IEnumerator Dash()
    {
        if (_currentStamina >= _staminaConsumption && _playerController.CanDash)
        {
            _currentStamina -= _staminaConsumption;
            if (_staminaBar != null)
            {
                _staminaBar.SetStamina(_currentStamina);
            }

            _playerController.CanDash = false;
            _playerController.IsDashing = true;

            if (_playerController.MoveDirection != Vector2.zero)
            {
                _rb.velocity = _playerController.MoveDirection * _dashSpeed;
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

            _playerController.IsDashing = false;

            _rb.velocity = Vector2.zero;

            yield return new WaitForSeconds(_dashCooldown);

            _playerController.CanDash = true;
        }
    }
}
