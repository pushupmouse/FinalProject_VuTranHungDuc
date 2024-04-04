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
    [SerializeField] private List<EquipmentTuple> _equipmentList = new List<EquipmentTuple>();
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private EquipmentData _equipmentData;
    private RarityData _rarityData;
    public Action OnEquip;

    public void SetTypeAndRarity(EquipmentType equipmentType, RarityType rarityType)
    {
        EquipmentTuple equipmentTuple = _equipmentList.Find(x => x.equipmentType == equipmentType);
        if (equipmentTuple == null)
        {
            return;
        }

        _equipmentData = equipmentTuple.equipmentData;

        _rarityData = _equipmentData.RarityDataList.Find(x => x.rarityType == rarityType);
        if (_rarityData == null)
        {
            return;
        }

        _spriteRenderer.sprite = _rarityData.image;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerEquipmentManager.Instance.EquipEquipment(_equipmentData, _rarityData);

            Destroy(gameObject);
        }
    }
}
