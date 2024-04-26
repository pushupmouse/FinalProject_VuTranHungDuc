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

[CreateAssetMenu(fileName = "New Attribute Data", menuName = "Attribute Data")]
public class AttributeData : ScriptableObject
{
    public float[] attributes = new float[System.Enum.GetNames(typeof(AttributeType)).Length];

    public float GetAttributeValue(AttributeType attributeType)
    {
        return attributes[(int)attributeType];
    }

    public void SetAttributeValue(AttributeType attributeType, float value)
    {
        attributes[(int)attributeType] = value;
    }
}

[CustomEditor(typeof(AttributeData))]
public class AttributeDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        AttributeData attributeData = (AttributeData)target;

        EditorGUI.BeginChangeCheck();

        for (int i = 0; i < attributeData.attributes.Length; i++)
        {
            AttributeType attributeType = (AttributeType)i;
            attributeData.attributes[i] = EditorGUILayout.FloatField(attributeType.ToString(), attributeData.attributes[i]);
        }

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }
    }
}