using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    public static PlayerEquipmentManager Instance;

    [SerializeField] private UnitStatsManager _unitStatsManager;
    [SerializeField] private RarityData _helmetRarityData;
    [SerializeField] private RarityData _chestRarityData;
    [SerializeField] private RarityData _shieldRarityData;
    [SerializeField] private RarityData _glovesRarityData;
    [SerializeField] private RarityData _bootsRarityData;

    private EquipmentData _helmetEquipment;
    private EquipmentData _chestEquipment;
    private EquipmentData _shieldEquipment;
    private EquipmentData _glovesEquipment;
    private EquipmentData _bootsEquipment;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EquipEquipment(EquipmentData equipmentData, RarityData rarityData)
    {
        switch (equipmentData.EquipmentType)
        {
            case EquipmentType.Helmet:
                if (_helmetEquipment != null)
                    return;
                _helmetEquipment = equipmentData;
                _helmetRarityData = rarityData;
                break;
            case EquipmentType.Chest:
                if (_chestEquipment != null)
                    return;
                _chestEquipment = equipmentData;
                _chestRarityData = rarityData;
                break;
            case EquipmentType.Shield:
                if (_shieldEquipment != null)
                    return;
                _shieldEquipment = equipmentData;
                _shieldRarityData = rarityData;
                break;
            case EquipmentType.Gloves:
                if (_glovesEquipment != null)
                    return;
                _glovesEquipment = equipmentData;
                _glovesRarityData = rarityData;
                break;
            case EquipmentType.Boots:
                if (_bootsEquipment != null)
                    return;
                _bootsEquipment = equipmentData;
                _bootsRarityData = rarityData;
                break;
            default:
                break;
        }

        _unitStatsManager.ModifyStat(equipmentData.PrimaryAttribute, rarityData.bonusAmount);
    }
}
