using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [SerializeField] private UnitStatsManager _unitStatsManager;
    [SerializeField] private EquipmentData _chest;
    [SerializeField] private EquipmentData _gloves;
    [SerializeField] private EquipmentData _boots;

    private EquipmentData _chestEquipment;
    private EquipmentData _glovesEquipment;
    private EquipmentData _bootsEquipment;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            EquipEquipment(_chest);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            EquipEquipment(_gloves);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            EquipEquipment(_boots);
        }
    }

    public void EquipEquipment(EquipmentData equipmentData)
    {
        switch (equipmentData.EquipmentType)
        {
            case EquipmentType.Chest:
                if (_chestEquipment != null)
                    return;
                _chestEquipment = equipmentData;
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

        _unitStatsManager.ModifyStat(equipmentData.MainAttribute, equipmentData.BonusAmount);
    }
}
