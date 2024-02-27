using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    [HideInInspector] public float MoveX, MoveY;
    [HideInInspector] public Vector2 MoveDirection;

    private void Update()
    {
        MoveX = Input.GetAxisRaw(HORIZONTAL);
        MoveY = Input.GetAxisRaw(VERTICAL);

        MoveDirection = new Vector2(MoveX, MoveY).normalized;
    }
}
