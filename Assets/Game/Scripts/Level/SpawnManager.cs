using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] private float _spawnChance = 0.5f;
    [SerializeField] private Transform _target;

    public static SpawnManager Instance;

    private List<Enemy> _spawnedEnemies = new List<Enemy>();
    private DungeonManager _dungeonManager;
    public Enemy Enemy;
    public bool EnemiesAlive = false;

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
            if (Random.value <= _spawnChance) // Check if random value is below spawn chance threshold
            {
                if(!EnemiesAlive)
                {
                    EnemiesAlive = true;
                }

                Enemy enemy = Instantiate(Enemy, spawnPoint.position, Quaternion.identity);

                enemy.OnEnemyDeath -= RemoveEnemy;
                enemy.OnEnemyDeath += RemoveEnemy;

                _spawnedEnemies.Add(enemy);

                enemy.SetTarget(_target);
            }
        }
    }

    private void RemoveEnemy(Enemy enemy)
    {
        _spawnedEnemies.Remove(enemy);

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
}
