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
    Regular = 0,
    Bronze = 1,
    Silver = 2,
    Gold = 3,
    Crimson = 4,
}

[CreateAssetMenu(fileName = "New Equipment Data", menuName = "Equipment Data")]
public class EquipmentData : ScriptableObject
{
    public EquipmentType EquipmentType;
    public AttributeType PrimaryAttribute;
    public AttributeType SecondaryAttribute;
    public RarityType RarityType;
    public Sprite Image;
    public float BonusAmount;
}
