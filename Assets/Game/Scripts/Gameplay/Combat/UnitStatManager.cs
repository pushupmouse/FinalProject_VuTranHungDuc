using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatManager : MonoBehaviour
{
    [SerializeField] private AttributeData attributeData;

    [HideInInspector] public float Constitution;
    [HideInInspector] public float Strength;
    [HideInInspector] public float Defense;
    [HideInInspector] public float Vitality;
    [HideInInspector] public float Accuracy;
    [HideInInspector] public float Dexterity;
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
        Vitality = attributeData.Vitality;
        Accuracy = attributeData.Accuracy;
        Dexterity = attributeData.Dexterity;
        Luck = attributeData.Luck;


        if (_healthController != null)
        {
            _healthController.InitializeHealth(Constitution);
            _healthController.InitializeDamageRed(Defense);
        }
        if (_attackController != null)
        {
            _attackController.InitializeDamage(Strength);
            _attackController.InitializeCritChance(Accuracy);
        }
    }
}
