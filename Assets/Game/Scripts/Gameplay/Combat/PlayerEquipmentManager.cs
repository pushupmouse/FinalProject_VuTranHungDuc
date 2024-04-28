using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    public static PlayerEquipmentManager Instance;

    //[SerializeField] private int _shards;

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

    public RarityData HelmetRarityData => _helmetRarityData;
    public RarityData ChestRarityData => _chestRarityData;
    public RarityData ShieldRarityData => _shieldRarityData;
    public RarityData GlovesRarityData => _glovesRarityData;
    public RarityData BootsRarityData => _bootsRarityData;

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

    public void OnInit()
    {
        _helmetRarityData = null;
        _chestRarityData = null;
        _shieldRarityData = null;
        _glovesRarityData = null;
        _bootsRarityData = null;
        _helmetEquipment = null;
        _chestEquipment = null;
        _shieldEquipment = null;
        _glovesEquipment = null;
        _bootsEquipment = null;
    }

    public void SetUnitStatManager(UnitStatsManager unitStatsManager)
    {
        _unitStatsManager = unitStatsManager;
    }

    public void EquipEquipment(EquipmentData equipmentData, RarityData rarityData)
    {
        RarityData currentRarityData = GetCurrentRarityData(equipmentData.EquipmentType);
        RarityData newRarityData = rarityData;
        // If there's already equipped equipment:
        // - and it's not null (i.e., there's currently equipped equipment)
        // - and it's not regular (lowest rank)
        // - and the new equipment is not of higher rarity than the current one
        if (currentRarityData != null && (int)newRarityData.rarityType <= (int)currentRarityData.rarityType)
        {
            // Increase shard count based on the rarity of the unequipped equipment
            ConvertDuplicateEquipment(newRarityData.coinDrop);
            return;
        }

        // If there's already equipped equipment, increase shard count based on its rarity
        if (currentRarityData != null)
        {
            ConvertDuplicateEquipment(currentRarityData.coinDrop);
        }

        EquipmentData currentEquipmentData = GetCurrentEquipmentData(equipmentData.EquipmentType);

        if (currentEquipmentData != null)
        {
            _unitStatsManager.ModifyStat(currentEquipmentData.PrimaryAttribute, -currentRarityData.primaryBonusAmount);
            _unitStatsManager.ModifyStat(currentEquipmentData.SecondaryAttribute, -currentRarityData.secondaryBonusAmount);
        }

        // Equip the new equipment
        switch (equipmentData.EquipmentType)
        {
            case EquipmentType.Helmet:
                _helmetEquipment = equipmentData;
                _helmetRarityData = rarityData;
                break;
            case EquipmentType.Chest:
                _chestEquipment = equipmentData;
                _chestRarityData = rarityData;
                break;
            case EquipmentType.Shield:
                _shieldEquipment = equipmentData;
                _shieldRarityData = rarityData;
                break;
            case EquipmentType.Gloves:
                _glovesEquipment = equipmentData;
                _glovesRarityData = rarityData;
                break;
            case EquipmentType.Boots:
                _bootsEquipment = equipmentData;
                _bootsRarityData = rarityData;
                break;
            default:
                break;
        }

        // Modify unit stats based on the new equipment
        _unitStatsManager.ModifyStat(equipmentData.PrimaryAttribute, rarityData.primaryBonusAmount);
        _unitStatsManager.ModifyStat(equipmentData.SecondaryAttribute, rarityData.secondaryBonusAmount);

        InventoryManager.Instance.UpdateImage(equipmentData.EquipmentType, rarityData);
    }

    private EquipmentData GetCurrentEquipmentData(EquipmentType equipmentType)
    {
        switch (equipmentType)
        {
            case EquipmentType.Helmet:
                return _helmetEquipment;
            case EquipmentType.Chest:
                return _chestEquipment;
            case EquipmentType.Shield:
                return _shieldEquipment;
            case EquipmentType.Gloves:
                return _glovesEquipment;
            case EquipmentType.Boots:
                return _bootsEquipment;
            default:
                return null;
        }
    }

    private RarityData GetCurrentRarityData(EquipmentType equipmentType)
    {
        switch (equipmentType)
        {
            case EquipmentType.Helmet:
                return _helmetRarityData;
            case EquipmentType.Chest:
                return _chestRarityData;
            case EquipmentType.Shield:
                return _shieldRarityData;
            case EquipmentType.Gloves:
                return _glovesRarityData;
            case EquipmentType.Boots:
                return _bootsRarityData;
            default:
                return null;
        }
    }

    //private void IncreaseShards(int amount)
    //{
    //    _shards += amount;
    //}

    private void ConvertDuplicateEquipment(int coinAmount)
    {
        CoinManager.Instance.AddCoins(coinAmount);
    }
}
