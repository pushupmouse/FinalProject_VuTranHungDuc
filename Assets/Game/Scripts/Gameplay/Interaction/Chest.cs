using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Coin _coin;
    [SerializeField] private Equipment _equipment;
    [SerializeField] private int _coinAmount = 10;
    [SerializeField] private int _equipmentAmount = 1;
    [SerializeField] private float _spawnRadius = 2f;
    
    private Transform _target;
    private List<Coin> _spawnedCoins = new List<Coin>();
    private List<Equipment> _spawnedEquipments = new List<Equipment>();
    public Action OnChestOpen;

    public void Interact()
    {
        Invoke(nameof(SpawnRewards), 1f);
        gameObject.layer = LayerMask.NameToLayer("Dead");
        _animator.Play("Open");
        Destroy(gameObject, 2f);
    }

    private void SpawnRewards()
    {
        for (int i = 0; i < _coinAmount; i++)
        {
            Vector3 randomOffset = Random.insideUnitCircle * _spawnRadius;
            Vector3 spawnPosition = transform.position + randomOffset;

            Coin coin = Instantiate(_coin, spawnPosition, Quaternion.identity);

            _spawnedCoins.Add(coin);

            coin.SetTarget(_target);
        }

        for (int i = 0; i < _equipmentAmount; i++)
        {
            Vector3 randomOffset = Random.insideUnitCircle * _spawnRadius;
            Vector3 spawnPosition = transform.position + randomOffset;

            Equipment equipment = Instantiate(_equipment, spawnPosition, Quaternion.identity);

            equipment.SetRandomTypeAndRarityRange(RarityType.Bronze, RarityType.Silver);

            equipment.SetTarget(_target);

            _spawnedEquipments.Add(equipment);
        }

        Invoke(nameof(CollectRewards), 1f);
    }

    private void CollectRewards()
    {
        foreach (Coin coin in _spawnedCoins)
        {
            if (coin != null)
            {
                coin.CollectCoins();
            }
        }

        foreach (Equipment equipment in _spawnedEquipments)
        {
            if (equipment != null)
            {
                equipment.CollectEquipments();
            }
        }

        OnChestOpen?.Invoke();
    }

    public void SetTarget(Transform transform)
    {
        _target = transform;
    }
}
