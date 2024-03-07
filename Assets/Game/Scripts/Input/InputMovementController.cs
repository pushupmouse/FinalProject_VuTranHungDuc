using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMovementController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    [HideInInspector] public float MoveX, MoveY;
    [HideInInspector] public Vector2 MoveDirection;

    private PlayerMovement _playerMovement;

    public bool CanDash = true;
    public bool IsDashing;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        MoveX = Input.GetAxisRaw(HORIZONTAL);
        MoveY = Input.GetAxisRaw(VERTICAL);

        MoveDirection = new Vector2(MoveX, MoveY).normalized;

        if (Input.GetKeyDown(KeyCode.LeftShift) && CanDash)
        {
            StartCoroutine(_playerMovement.Dash());
        }
    }
}
