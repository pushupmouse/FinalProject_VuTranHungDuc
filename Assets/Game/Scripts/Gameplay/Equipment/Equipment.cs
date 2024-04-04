using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public EquipmentData equipmentData;
    public SpriteRenderer spriteRenderer;

    // Other fields and methods related to equipment...

    private void Start()
    {
        SetAppearance();
    }

    public void SetAppearance()
    {
        //if (equipmentData != null && equipmentData.Image != null)
        //{
        //    // Set appearance using the sprite from EquipmentData
        //    // For example, if you're setting the appearance of a SpriteRenderer component:
        //    if (spriteRenderer != null)
        //    {
        //        spriteRenderer.sprite = equipmentData.Image;
        //    }
        //}
        //else
        //{
        //    Debug.LogWarning("EquipmentData or Image is missing.");
        //}
    }

    public Equipment GetRandomEquipment()
    {
        return null;
    }
}
