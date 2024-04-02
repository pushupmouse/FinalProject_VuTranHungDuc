using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [SerializeField, Range(0f, 1f)] private float _spawnChance = 0.5f;
    [SerializeField] private Transform _target;
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Coin _coin;
    [SerializeField] private Chest _chest;

    [HideInInspector] public bool EnemiesAlive = false;

    private List<Enemy> _spawnedEnemies = new List<Enemy>();
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
            if (Random.value <= _spawnChance)
            {
                if(!EnemiesAlive)
                {
                    EnemiesAlive = true;
                }

                Enemy enemy = Instantiate(_enemy, spawnPoint.position, Quaternion.identity);

                enemy.OnEnemyDeath -= RemoveEnemy;
                enemy.OnEnemyDeath += RemoveEnemy;

                _spawnedEnemies.Add(enemy);

                enemy.SetTarget(_target);
            }
        }
    }

    public void SpawnTreasure()
    {
        if (_dungeonManager.CurrentPlayerLocation.IsCleared)
        {
            return;
        }

        Chest chest = Instantiate(_chest, Vector2.zero, Quaternion.identity);

        chest.OnChestOpen -= OnChestOpenHandler;
        chest.OnChestOpen += OnChestOpenHandler;
    }

    private void RemoveEnemy(Enemy enemy)
    {
        _spawnedEnemies.Remove(enemy);

        SpawnCoin(enemy);

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
    }

    private void SpawnCoin(Enemy enemy)
    {
        Instantiate(_coin, enemy.transform.position, Quaternion.identity);
    }

    private void OnChestOpenHandler()
    {
        if (!_dungeonManager.CurrentPlayerLocation.IsCleared)
        {
            _dungeonManager.CurrentPlayerLocation.IsCleared = true;
        }
    }
}
