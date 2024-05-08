using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum AttributeType
{
    Constitution = 0,
    Strength = 1,
    Defense = 2,
    Intensity = 3,
    Accuracy = 4,
    Resilience = 5,
}

[System.Serializable]
public class Attribute
{
    public AttributeType type;
    public float value;
}

[CreateAssetMenu(fileName = "New Attribute Data", menuName = "Attribute Data")]
public class AttributeData : ScriptableObject
{
    public List<Attribute> attributes = new List<Attribute>();

    public float GetAttributeValue(AttributeType attributeType)
    {
        foreach (Attribute attribute in attributes)
        {
            if (attribute.type == attributeType)
            {
                return attribute.value;
            }
        }
        return 0f; // Default value if attribute type not found
    }

    public void SetAttributeValue(AttributeType attributeType, float value)
    {
        foreach (Attribute attribute in attributes)
        {
            if (attribute.type == attributeType)
            {
                attribute.value = value;
                return;
            }
        }
        // If attribute type not found, add it
        attributes.Add(new Attribute { type = attributeType, value = value });
    }
}