using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [SerializeField] private UnitStatsManager _unitStatsManager;

    private EquipmentData _helmetEquipment;
    private EquipmentData _chestEquipment;
    private EquipmentData _shieldEquipment;
    private EquipmentData _glovesEquipment;
    private EquipmentData _bootsEquipment;

    public void EquipEquipment(EquipmentData equipmentData)
    {
        switch (equipmentData.EquipmentType)
        {
            case EquipmentType.Helmet:
                if (_helmetEquipment != null)
                    return;
                _helmetEquipment = equipmentData;
                break;
            case EquipmentType.Chest:
                if (_chestEquipment != null)
                    return;
                _chestEquipment = equipmentData;
                break;
            case EquipmentType.Shield:
                if (_shieldEquipment != null)
                    return;
                _shieldEquipment = equipmentData;
                break;
            case EquipmentType.Gloves:
                if (_glovesEquipment != null)
                    return;
                _glovesEquipment = equipmentData;
                break;
            case EquipmentType.Boots:
                if (_bootsEquipment != null)
                    return;
                _bootsEquipment = equipmentData;
                break;
            default:
                break;
        }

        //_unitStatsManager.ModifyStat(equipmentData.PrimaryAttribute, equipmentData.BonusAmount);
    }
}
