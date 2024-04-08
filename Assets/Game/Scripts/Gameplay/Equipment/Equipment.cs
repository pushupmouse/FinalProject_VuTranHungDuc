using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

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
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _speed = 10f;

    private EquipmentData _equipmentData;
    private RarityData _rarityData;
    private Transform _target;
    private bool _toCollect;
    public Action OnEquip;

    private void FixedUpdate()
    {
        if (!_toCollect)
        {
            return;
        }

        if (_target != null)
        {
            Vector3 direction = (_target.position - transform.position).normalized;

            Vector3 targetVelocity = direction * _speed;

            _rb.velocity = targetVelocity;
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void CollectEquipments()
    {
        _toCollect = true;
    }

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

    public void SetRandomTypeAndRarity(RarityType minRarityType, RarityType maxRarityType)
    {
        int randomEquipmentIndex = Random.Range(0, Enum.GetValues(typeof(EquipmentType)).Length);

        EquipmentTuple equipmentTuple = _equipmentList.Find(x => x.equipmentType == (EquipmentType)randomEquipmentIndex);
        if (equipmentTuple == null)
        {
            return;
        }

        _equipmentData = equipmentTuple.equipmentData;

        int minRarityInt = (int)minRarityType;
        int maxRarityInt = (int)maxRarityType;

        RarityType randomRarityType = (RarityType)Random.Range(minRarityInt, maxRarityInt + 1);

        _rarityData = _equipmentData.RarityDataList.Find(x => x.rarityType == randomRarityType);
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
