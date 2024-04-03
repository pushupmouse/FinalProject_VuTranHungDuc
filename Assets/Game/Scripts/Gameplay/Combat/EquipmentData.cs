using UnityEngine;

public enum EquipmentType
{
    Chest = 0,
    Gloves = 1,
    Boots = 2
}

[CreateAssetMenu(fileName = "New Equipment Data", menuName = "Equipment Data")]
public class EquipmentData : ScriptableObject
{
    public EquipmentType EquipmentType;
    public AttributeType MainAttribute;
    public float BonusAmount;
}
