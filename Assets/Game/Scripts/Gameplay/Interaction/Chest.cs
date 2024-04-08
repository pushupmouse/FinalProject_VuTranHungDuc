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
    public Action OnChestOpen;

    public void Interact()
    {
        Invoke(nameof(SpawnRewards), 1f);
        OnChestOpen?.Invoke();
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

            Instantiate(_coin, spawnPosition, Quaternion.identity);
        }

        for (int i = 0; i < _equipmentAmount; i++)
        {
            Vector3 randomOffset = Random.insideUnitCircle * _spawnRadius;
            Vector3 spawnPosition = transform.position + randomOffset;

            Equipment equipment = Instantiate(_equipment, spawnPosition, Quaternion.identity);

            equipment.SetRandomTypeAndRarity(RarityType.Bronze, RarityType.Gold);
        }
    }
}
