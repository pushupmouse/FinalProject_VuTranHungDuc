using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMovementController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    [SerializeField] private float _hitDuration = 0.2f;

    [HideInInspector] public float MoveX, MoveY;
    [HideInInspector] public Vector2 MoveDirection;
    [HideInInspector] public bool CanDash = true;
    [HideInInspector] public bool IsDashing;
    [HideInInspector] public bool IsTakeDamage;
    [HideInInspector] public bool IsDead;

    private HealthController _health;
    private PlayerMovement _playerMovement;

    private void Awake()
    {
        _health = GetComponent<HealthController>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        _health.OnDeath -= HandleDeath;
        _health.OnDeath += HandleDeath;
        _health.OnTakeDamage -= HandleTakeDamage;
        _health.OnTakeDamage += HandleTakeDamage;

        IsDead = false;
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

    private void HandleTakeDamage()
    {
        IsTakeDamage = true;
        StartCoroutine(ResetTakeDamage());
    }

    private IEnumerator ResetTakeDamage()
    {
        yield return new WaitForSeconds(_hitDuration);
        IsTakeDamage = false;
    }

    private void HandleDeath()
    {
        IsDead = true;
        gameObject.layer = LayerMask.NameToLayer("Dead");
        Destroy(gameObject, 2f);
    }
}
