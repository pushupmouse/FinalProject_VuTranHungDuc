using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cthullu : Enemy
{
    public override void UseFirstSkill()
    {
        SpawnManager.Instance.SpawnAdditionalBoss(transform, 0);
    }

    public override void UseSecondSkill()
    {
        SpawnManager.Instance.SpawnAdditionalBoss(transform, 1);
    }
}
