using UnityEngine;

public class UnitStatsManager : MonoBehaviour
{
    [SerializeField] private AttributeData attributeData;
    [SerializeField] private float _constitution;
    [SerializeField] private float _strength;
    [SerializeField] private float _defense;
    [SerializeField] private float _accuracy;
    [SerializeField] private float _resilience;

    private HealthController healthController;
    private AttackController attackController;


    private void Awake()
    {
        healthController = GetComponent<HealthController>();
        attackController = GetComponent<AttackController>();
    }

    private void Start()
    {
        InitializeStats();
    }

    private void InitializeStats()
    {
        _constitution = attributeData.GetAttributeValue(AttributeType.Constitution);
        _strength = attributeData.GetAttributeValue(AttributeType.Strength);
        _defense = attributeData.GetAttributeValue(AttributeType.Defense);
        _accuracy = attributeData.GetAttributeValue(AttributeType.Accuracy);
        _resilience = attributeData.GetAttributeValue(AttributeType.Resilience);

        ApplyStats();
    }

    private void ApplyStats()
    {
        if (healthController != null)
        {
            healthController.InitializeHealth(_constitution);
            healthController.InitializeDamageRed(_defense);
            healthController.InitializeRecoveryChance(_resilience);
        }

        if (attackController != null)
        {
            attackController.InitializeDamage(_strength);
            attackController.InitializeCritChance(_accuracy);
        }
    }

    public void ModifyStat(AttributeType attributeType, float amount)
    {
        switch (attributeType)
        {
            case AttributeType.Constitution:
                _constitution += amount;
                if (healthController != null)
                    healthController.InitializeHealth(_constitution);
                break;
            case AttributeType.Strength:
                _strength += amount;
                if (attackController != null)
                    attackController.InitializeDamage(_strength);
                break;
            case AttributeType.Defense:
                _defense += amount;
                if (healthController != null)
                    healthController.InitializeDamageRed(_defense);
                break;
            case AttributeType.Accuracy:
                _accuracy += amount;
                if (attackController != null)
                    attackController.InitializeCritChance(_accuracy);
                break;
            case AttributeType.Resilience:
                _resilience += amount;
                if (healthController != null)
                    healthController.InitializeRecoveryChance(_resilience);
                break;
            default:
                break;
        }
    }
}