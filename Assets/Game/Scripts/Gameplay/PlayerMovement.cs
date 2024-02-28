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

    private InputMovementController _movementController;
    private InputAttackController _attackController;

    private void Awake()
    {
        _movementController = GetComponent<InputMovementController>();
        _attackController = GetComponent<InputAttackController>();
    }

    private void FixedUpdate()
    {
        if(_attackController.IsAttacking) return;

        _rb.MovePosition(_rb.position + _movementController.MoveDirection * _moveSpeed * Time.fixedDeltaTime);

        //Flip the character
        if (_movementController.MoveX > 0.1f)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else if (_movementController.MoveX < -0.1f)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
    }
}
