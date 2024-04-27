using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEvent : MonoBehaviour
{
    [SerializeField] private AttackController _attackController;

    public void TriggerAttack()
    {
        _attackController.Attack();
    }
}
