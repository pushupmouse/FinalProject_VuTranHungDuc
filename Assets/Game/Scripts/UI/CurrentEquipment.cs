using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentEquipment : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private List<EquipmentData> _equipmentData;

    private PlayerEquipmentManager playerEquipmentManager;

    private void Start()
    {
        playerEquipmentManager = PlayerEquipmentManager.Instance;
    }

    public void SetImage(EquipmentType equipmentType, RarityData rarityData)
    {
        //RarityData rarityData = GetEquipmentRarity(equipmentType);
        _image.sprite = rarityData.image;
    }

    private RarityData GetEquipmentRarity(EquipmentType equipmentType)
    {
        switch (equipmentType)
        {
            case EquipmentType.Helmet:
                return playerEquipmentManager.HelmetRarityData;
            case EquipmentType.Chest:
                return playerEquipmentManager.ChestRarityData;
            case EquipmentType.Shield:
                return playerEquipmentManager.ShieldRarityData;
            case EquipmentType.Gloves:
                return playerEquipmentManager.GlovesRarityData;
            case EquipmentType.Boots:
                return playerEquipmentManager.BootsRarityData;
            default:
                return null;
        }
    }
}
