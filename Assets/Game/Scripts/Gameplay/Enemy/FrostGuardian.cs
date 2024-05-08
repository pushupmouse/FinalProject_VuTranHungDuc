using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostGuardian : Enemy
{
    public int SpawnIndex = 0;

    public override void UseFirstSkill()
    {
        SpawnManager.Instance.SpawnBossCopy(transform, SpawnIndex);
    }

    public override void UseSecondSkill()
    {
        UseFirstSkill();
    }
}
