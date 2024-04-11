using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Shopkeeper : MonoBehaviour, IInteractable
{
    private Transform _target;

    public void Interact()
    {
        UIManager.Instance.ToggleShopkeeperCanvas();
    }

    public void SetTarget(Transform transform)
    {
        _target = transform;
    }
}
