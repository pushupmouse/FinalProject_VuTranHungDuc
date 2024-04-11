using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Priestess : MonoBehaviour, IInteractable
{
    private Transform _target;

    public void Interact()
    {
        if (!SpawnManager.Instance.Healed)
        {
            RecoverTargetHealth();
        }
    }

    public void SetTarget(Transform transform)
    {
        _target = transform;
    }

    private void RecoverTargetHealth()
    {
        if (_target == null)
        {
            return;
        }

        HealthController healthController = _target.GetComponent<HealthController>();

        StartCoroutine(healthController.HealOverTime(healthController.MaxHealth * 0.1f, 1f));

        SpawnManager.Instance.Healed = true;
    }
}
