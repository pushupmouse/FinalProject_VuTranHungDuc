using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Helmet = 0,
    Chest = 1,
    Shield = 2,
    Gloves = 3,
    Boots = 4
}

public enum RarityType
{
    None = 0,
    Regular = 1,
    Bronze = 2,
    Silver = 3,
    Gold = 4,
    Crimson = 5,
}

[System.Serializable]
public class RarityData
{
    public RarityType rarityType;
    public Sprite image;
    public float primaryBonusAmount;
    public float secondaryBonusAmount;
    public int coinDrop;
}

[CreateAssetMenu(fileName = "New Equipment Data", menuName = "Equipment Data")]
public class EquipmentData : ScriptableObject
{
    public EquipmentType EquipmentType;
    public AttributeType PrimaryAttribute;
    public AttributeType SecondaryAttribute;

    public List<RarityData> RarityDataList = new List<RarityData>();
}