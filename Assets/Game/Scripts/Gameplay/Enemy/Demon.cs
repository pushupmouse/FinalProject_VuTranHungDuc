using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : Enemy
{
    public override void UseFirstSkill()
    {
        SpawnManager.Instance.SpawnFireballSpawner(0);
    }

    public override void UseSecondSkill()
    {
        SpawnManager.Instance.SpawnFireballSpawner(1);
    }

    protected override void HandleDeath()
    {
        IsDead = true;
        gameObject.layer = LayerMask.NameToLayer("Dead");
        SpawnManager.Instance.DespawnFireballSpawner();
        OnEnemyDeath?.Invoke(this);
        Destroy(gameObject, 2f);
    }
}
