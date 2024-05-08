using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballSpawner : MonoBehaviour
{
    [SerializeField] private Fireball _fireball;
    [SerializeField] private float _minSpawnInterval = 2f;
    [SerializeField] private float _maxSpawnInterval = 5f;

    void Start()
    {
        SpawnFireball();
    }

    private void SpawnFireball()
    {
        Instantiate(_fireball, transform.position, transform.rotation);

        Invoke(nameof(SpawnFireball), Random.Range(_minSpawnInterval, _maxSpawnInterval));
    }
}
