using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [SerializeField, Range(0f, 1f)] private float _enemySpawnRate = 0.5f;
    [SerializeField, Range(0f, 1f)] private float _equipmentSpawnRate = 0.1f;
    [SerializeField] private Transform _target;
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Enemy _bossEnemy;
    [SerializeField] private Coin _coin;
    [SerializeField] private Equipment _equipment;
    [SerializeField] private Chest _chest;
    [SerializeField] private Blacksmith _blacksmith;
    [SerializeField] private Shopkeeper _shopkeeper;
    [SerializeField] private Priestess _priestess;

    [HideInInspector] public bool EnemiesAlive = false;
    [HideInInspector] public bool RewardsToCollect = false;

    private List<Enemy> _spawnedEnemies = new List<Enemy>();
    private List<Coin> _spawnedCoins = new List<Coin>();
    private List<Equipment> _spawnedEquipments = new List<Equipment>();
    private DungeonManager _dungeonManager;
    public bool Healed = false;
    
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
        OnInit();
        _dungeonManager = DungeonManager.Instance;
    }

    private void OnInit()
    {
        Healed = false;
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

        if(!EnemiesAlive)
        {
            SpawnTreasure(room);
        }
    }


    public void SpawnBoss(Room room)
    {
        if (_dungeonManager.CurrentPlayerLocation.IsCleared)
        {
            return;
        }

        if (!EnemiesAlive)
        {
            EnemiesAlive = true;
        }

        Enemy bossEnemy = Instantiate(_bossEnemy, room.SpawnPoints[0].position, Quaternion.identity);

        bossEnemy.OnEnemyDeath -= OnBossDeathHandler;
        bossEnemy.OnEnemyDeath += OnBossDeathHandler;

        bossEnemy.transform.SetParent(room.SpawnInstances);

        _spawnedEnemies.Add(bossEnemy);

        bossEnemy.SetTarget(_target);
    }

    public void SpawnTreasure(Room room)
    {
        if (_dungeonManager.CurrentPlayerLocation.IsCleared)
        {
            return;
        }

        RewardsToCollect = true;

        Chest chest = Instantiate(_chest, Vector2.zero, Quaternion.identity);

        chest.OnChestOpen -= OnChestOpenHandler;
        chest.OnChestOpen += OnChestOpenHandler;

        chest.transform.SetParent(room.SpawnInstances);

        chest.SetTarget(_target);
    }

    public void SpawnUpgradeNPCs(Room room)
    {
        Blacksmith blacksmith = Instantiate(_blacksmith, room.SpawnPoints[0].position, Quaternion.identity);
        Shopkeeper shopkeeper = Instantiate(_shopkeeper, room.SpawnPoints[2].position, Quaternion.identity);
        Priestess priestess = Instantiate(_priestess, room.SpawnPoints[3].position, Quaternion.identity);

        blacksmith.transform.SetParent(room.SpawnInstances);
        shopkeeper.transform.SetParent(room.SpawnInstances);
        priestess.transform.SetParent(room.SpawnInstances);

        blacksmith.SetTarget(_target);
        shopkeeper.SetTarget(_target);
        priestess.SetTarget(_target);
    }

    private void OnEnemyDeathHandler(Enemy enemy)
    {
        _spawnedEnemies.Remove(enemy);

        SpawnCoin(enemy);

        if (Random.value <= _equipmentSpawnRate)
        {
            SpawnEquipment(enemy, RarityType.Regular, RarityType.Bronze);
        }

        if (_spawnedEnemies.Count == 0)
        {
            OnAllEnemiesDefeated();
        }
    }

    private void OnBossDeathHandler(Enemy enemy)
    {
        _spawnedEnemies.Remove(enemy);

        Invoke(nameof(SpawnBossRewards), 2f);

        OnBossDefeated();
    }

    private void SpawnBossRewards()
    {
        for (int i = 0; i < 20; i++)
        {
            Vector3 randomOffset = Random.insideUnitCircle * 3;
            Vector3 spawnPosition = transform.position + randomOffset;

            Coin coin = Instantiate(_coin, spawnPosition, Quaternion.identity);

            _spawnedCoins.Add(coin);

            coin.SetTarget(_target);
        }

        for (int i = 0; i < 2; i++)
        {
            Vector3 randomOffset = Random.insideUnitCircle * 3;
            Vector3 spawnPosition = transform.position + randomOffset;

            Equipment equipment = Instantiate(_equipment, spawnPosition, Quaternion.identity);

            equipment.SetRandomTypeAndRarityRange(RarityType.Silver, RarityType.Gold);

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

        foreach (Equipment equipment in _spawnedEquipments)
        {
            if (equipment != null)
            {
                equipment.CollectEquipments();
            }
        }
    }

    private void OnBossDefeated()
    {
        EnemiesAlive = false;

        if (!_dungeonManager.CurrentPlayerLocation.IsCleared)
        {
            _dungeonManager.CurrentPlayerLocation.IsCleared = true;
        }

        //spawn way to new level
    }

    private void SpawnCoin(Enemy enemy)
    {
        Vector3 directionToPlayer = _target.transform.position - enemy.transform.position;

        Vector3 spawnPosition = enemy.transform.position - directionToPlayer.normalized * 1f;

        Coin coin = Instantiate(_coin, spawnPosition, Quaternion.identity);

        coin.SetTarget(_target);

        _spawnedCoins.Add(coin);
    }

    private void SpawnEquipment(Enemy enemy, RarityType minRarity, RarityType maxRarity)
    {
        Vector3 directionToPlayer = _target.transform.position - enemy.transform.position;

        Vector3 spawnPosition = enemy.transform.position - directionToPlayer.normalized * 1.5f;

        Equipment equipment = Instantiate(_equipment, spawnPosition, Quaternion.identity);

        equipment.SetTarget(_target);

        _spawnedEquipments.Add(equipment);

        equipment.SetRandomTypeAndRarityRange(minRarity, maxRarity);
    }

    private void OnChestOpenHandler()
    {
        RewardsToCollect = false;

        if (!_dungeonManager.CurrentPlayerLocation.IsCleared)
        {
            _dungeonManager.CurrentPlayerLocation.IsCleared = true;
        }
    }
}
