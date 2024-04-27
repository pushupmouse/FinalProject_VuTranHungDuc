using UnityEngine;

public class UnitStatsManager : MonoBehaviour
{
    [SerializeField] private AttributeData attributeData;
    [SerializeField] private float _constitution;
    [SerializeField] private float _strength;
    [SerializeField] private float _defense;
    [SerializeField] private float _intensity;
    [SerializeField] private float _accuracy;
    [SerializeField] private float _resilience;
    [SerializeField] bool _isEnemy;

    private HealthController healthController;
    private AttackController attackController;
    private float _statIncrease = 0;


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
        if (_isEnemy)
        {
            _statIncrease = LevelManager.Instance.GetLevelData().StatIncrease;
        }
        else
        {
            _statIncrease = 0;
        }

        _constitution = attributeData.GetAttributeValue(AttributeType.Constitution) * (1 + _statIncrease);
        _strength = attributeData.GetAttributeValue(AttributeType.Strength) * (1 + _statIncrease);
        _defense = attributeData.GetAttributeValue(AttributeType.Defense) * (1 + _statIncrease);
        _intensity = attributeData.GetAttributeValue(AttributeType.Intensity) * (1 + _statIncrease);
        _accuracy = attributeData.GetAttributeValue(AttributeType.Accuracy) * (1 + _statIncrease);
        _resilience = attributeData.GetAttributeValue(AttributeType.Resilience) * (1 + _statIncrease);

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
            attackController.InitializeDamageMult(_intensity);
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
            case AttributeType.Intensity:
                _intensity += amount;
                if(attackController != null)
                    attackController.InitializeDamageMult(_intensity);
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