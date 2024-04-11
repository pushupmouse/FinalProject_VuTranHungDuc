using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private Canvas _shopkeeperCanvas;
    
    private bool _isShopkeeperActive = false;

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
        _shopkeeperCanvas.enabled = false;
        _isShopkeeperActive = false;

        ShopManager.Instance.PopulateShop();
    }

    public void ToggleShopkeeperCanvas()
    {
        _isShopkeeperActive = !_isShopkeeperActive;
        _shopkeeperCanvas.enabled = _isShopkeeperActive;
    }


}
