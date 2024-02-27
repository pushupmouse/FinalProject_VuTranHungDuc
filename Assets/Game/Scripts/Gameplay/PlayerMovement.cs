using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerInputController))]
[RequireComponent(typeof(PlayerAttackController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _moveSpeed;

    private PlayerInputController _playerController;
    private PlayerAttackController _attackController;

    private void Awake()
    {
        _playerController = GetComponent<PlayerInputController>();
        _attackController = GetComponent<PlayerAttackController>();
    }

    private void FixedUpdate()
    {
        if(_attackController.IsAttacking) return;

        _rb.MovePosition(_rb.position + _playerController.MoveDirection * _moveSpeed * Time.fixedDeltaTime);

        //Flip the character
        if (_playerController.MoveX > 0.1f)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else if (_playerController.MoveX < -0.1f)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
    }
}
