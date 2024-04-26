using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Shopkeeper : MonoBehaviour, IInteractable
{
    private Transform _target;
    private bool _isShopActive = false;

    private void Update()
    {
      if(_isShopActive)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Interact();
            }
        }   
    }

    public void Interact()
    {
        UIManager.Instance.ToggleShopkeeperCanvas();
        _isShopActive = !_isShopActive;
    }

    public void SetTarget(Transform transform)
    {
        _target = transform;
    }
}
