using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatManager : MonoBehaviour
{
    [SerializeField] private AttributeData attributeData;

    [HideInInspector] public float Constitution;
    [HideInInspector] public float Strength;
    [HideInInspector] public float Defense;
    [HideInInspector] public float Fortune;
    [HideInInspector] public float Accuracy;
    [HideInInspector] public float Resilience;
    [HideInInspector] public float Luck;

    private HealthController _healthController;
    private AttackController _attackController;

    private void Awake()
    {
        _healthController = GetComponent<HealthController>();
        _attackController = GetComponent<AttackController>();
    }

    private void Start()
    {
        Constitution = attributeData.Constitution;
        Strength = attributeData.Strength;
        Defense = attributeData.Defense;
        Fortune = attributeData.Fortune;
        Accuracy = attributeData.Accuracy;
        Resilience = attributeData.Resilience;
        Luck = attributeData.Luck;


        if (_healthController != null)
        {
            _healthController.InitializeHealth(Constitution);
            _healthController.InitializeDamageRed(Defense);
            _healthController.InitializeRecoveryChance(Resilience);
        }
        if (_attackController != null)
        {
            _attackController.InitializeDamage(Strength);
            _attackController.InitializeCritChance(Accuracy);
        }

        _healthController.OnInit();
    }
}
