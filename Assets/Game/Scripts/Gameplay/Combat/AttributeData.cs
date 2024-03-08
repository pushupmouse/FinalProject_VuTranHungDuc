using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attribute Data", menuName = "Attribute Data")]
public class AttributeData : ScriptableObject
{
    [Header("Primary Attributes")]
    public float Constitution;
    public float Strength;
    public float Defense;

    [Header("Secondary Attributes")]
    public float Vitality;
    public float Accuracy;
    public float Dexterity;
    public float Luck;
}
