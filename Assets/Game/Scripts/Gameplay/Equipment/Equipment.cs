using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EquipmentTuple
{
    public EquipmentType equipmentType;
    public EquipmentData equipmentData;
}

public class Equipment : MonoBehaviour
{
    public List<EquipmentTuple> equipmentList = new List<EquipmentTuple>();
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        SetRandomAppearance();
    }

    private void SetAppearance(EquipmentType equipmentType, RarityType rarityType)
    {
        int equipmentIndex = (int)equipmentType;
        if (equipmentIndex < 0 || equipmentIndex >= equipmentList.Count)
        {
            return;
        }

        EquipmentData equipmentData = equipmentList[equipmentIndex].equipmentData;

        int rarityIndex = (int)rarityType;
        if (rarityIndex < 0 || rarityIndex >= equipmentData.RarityDataList.Count)
        {
            return;
        }

        RarityData rarityData = equipmentData.RarityDataList[rarityIndex];

        spriteRenderer.sprite = rarityData.image;
    }

    private void SetRandomAppearance()
    {
        // Generate random indices for equipment type and rarity type
        int randomEquipmentIndex = UnityEngine.Random.Range(0, equipmentList.Count);
        int randomRarityIndex = UnityEngine.Random.Range(0, Enum.GetValues(typeof(RarityType)).Length);

        // Check if the random indices are within bounds
        if (randomEquipmentIndex < 0 || randomEquipmentIndex >= equipmentList.Count ||
            randomRarityIndex < 0 || randomRarityIndex >= equipmentList[randomEquipmentIndex].equipmentData.RarityDataList.Count)
        {
            Debug.LogWarning("Invalid random indices generated.");
            return;
        }

        // Get the corresponding equipment data and rarity data
        EquipmentData equipmentData = equipmentList[randomEquipmentIndex].equipmentData;
        RarityData rarityData = equipmentData.RarityDataList[randomRarityIndex];

        // Set the sprite renderer's sprite to the random rarity's image
        spriteRenderer.sprite = rarityData.image;
    }
}
