using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [SerializeField, Range(0f, 1f)] private float _enemySpawnRate = 0.5f;
    [SerializeField, Range(0f, 1f)] private float _equipmentSpawnRate = 0.1f;
    [SerializeField] private Transform _target;
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Coin _coin;
    [SerializeField] private Equipment _equipment;
    [SerializeField] private Chest _chest;

    [HideInInspector] public bool EnemiesAlive = false;

    private List<Enemy> _spawnedEnemies = new List<Enemy>();
    private List<Coin> _spawnedCoins = new List<Coin>();
    private List<Equipment> _spawnedEquipment = new List<Equipment>();
    private DungeonManager _dungeonManager;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _dungeonManager = DungeonManager.Instance;
    }

    public void SpawnEnemy(Room room)
    {
        if (_dungeonManager.CurrentPlayerLocation.IsCleared)
        {
            return;
        }

        foreach (Transform spawnPoint in room.SpawnPoints)
        {
            if (Random.value <= _enemySpawnRate)
            {
                if(!EnemiesAlive)
                {
                    EnemiesAlive = true;
                }

                Enemy enemy = Instantiate(_enemy, spawnPoint.position, Quaternion.identity);

                enemy.OnEnemyDeath -= OnEnemyDeathHandler;
                enemy.OnEnemyDeath += OnEnemyDeathHandler;

                enemy.transform.SetParent(room.SpawnInstances);

                _spawnedEnemies.Add(enemy);

                enemy.SetTarget(_target);
            }
        }
    }

    public void SpawnTreasure(Room room)
    {
        if (_dungeonManager.CurrentPlayerLocation.IsCleared)
        {
            return;
        }

        Chest chest = Instantiate(_chest, Vector2.zero, Quaternion.identity);

        chest.OnChestOpen -= OnChestOpenHandler;
        chest.OnChestOpen += OnChestOpenHandler;

        chest.transform.SetParent(room.SpawnInstances);
    }

    private void OnEnemyDeathHandler(Enemy enemy)
    {
        _spawnedEnemies.Remove(enemy);

        SpawnCoin(enemy);

        if (Random.value <= _equipmentSpawnRate)
        {
            SpawnEquipment(enemy);
        }

        if (_spawnedEnemies.Count == 0)
        {
            OnAllEnemiesDefeated();
        }
    }

    private void OnAllEnemiesDefeated()
    {
        EnemiesAlive = false;

        if (!_dungeonManager.CurrentPlayerLocation.IsCleared)
        {
            _dungeonManager.CurrentPlayerLocation.IsCleared = true;
        }

        foreach (Coin coin in _spawnedCoins)
        {
            if (coin != null)
            {
                coin.CollectCoins();
            }
        }

        foreach (Equipment equipment in _spawnedEquipment)
        {
            if (equipment != null)
            {
                equipment.CollectEquipments();
            }
        }
    }

    public void SpawnCoin(Transform transform)
    {
        Coin coin = Instantiate(_coin, transform.position, Quaternion.identity);

        coin.SetTarget(_target);

        coin.CollectCoins();

        _spawnedCoins.Add(coin);
    }

    private void SpawnCoin(Enemy enemy)
    {
        Vector3 directionToPlayer = _target.transform.position - enemy.transform.position;

        Vector3 spawnPosition = enemy.transform.position - directionToPlayer.normalized * 1f;

        Coin coin = Instantiate(_coin, spawnPosition, Quaternion.identity);

        coin.SetTarget(_target);

        _spawnedCoins.Add(coin);
    }

    private void SpawnEquipment(Enemy enemy)
    {
        Vector3 directionToPlayer = _target.transform.position - enemy.transform.position;

        Vector3 spawnPosition = enemy.transform.position - directionToPlayer.normalized * 1.5f;

        Equipment equipment = Instantiate(_equipment, spawnPosition, Quaternion.identity);

        equipment.SetTarget(_target);

        _spawnedEquipment.Add(equipment);

        equipment.SetRandomTypeAndRarity(RarityType.Regular, RarityType.Bronze);
    }

    private void OnChestOpenHandler()
    {
        if (!_dungeonManager.CurrentPlayerLocation.IsCleared)
        {
            _dungeonManager.CurrentPlayerLocation.IsCleared = true;
        }
    }
}
