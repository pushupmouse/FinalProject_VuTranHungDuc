using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator _animator;

    public Action OnChestOpen;

    public void Interact()
    {
        CoinManager.Instance.AddCoins(25);
        OnChestOpen?.Invoke();
        gameObject.layer = LayerMask.NameToLayer("Dead");
        _animator.Play("Open");
        Destroy(gameObject, 2f);
    }
}
