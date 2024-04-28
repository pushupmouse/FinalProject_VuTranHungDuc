using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private Canvas _shopkeeperCanvas;
    [SerializeField] private Canvas _inventoryCanvas;
    [SerializeField] private Canvas _gameOverScreenCanvas;
    
    private bool _isShopkeeperActive = false;
    private bool _isInventoryActive = false;

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
    }

    public void OnInit()
    {
        _shopkeeperCanvas.enabled = false;
        _isShopkeeperActive = false;
        ShopManager.Instance.PopulateShop();

        _inventoryCanvas.enabled = false;
        _isInventoryActive = false;
        InventoryManager.Instance.PopulateInventory();
        InventoryManager.Instance.SetCoinText(0);

        _gameOverScreenCanvas.enabled = false;
    }

    public void ToggleShopkeeperCanvas()
    {
        _isShopkeeperActive = !_isShopkeeperActive;
        _shopkeeperCanvas.enabled = _isShopkeeperActive;

        if (_isShopkeeperActive != _isInventoryActive)
        {
            ToggleInventoryCanvas();
        }
    }

    public void ToggleInventoryCanvas()
    {
        _isInventoryActive = !_isInventoryActive;
        _inventoryCanvas.enabled = _isInventoryActive;
    }

    public void ShowGameOverScreen()
    {
        _gameOverScreenCanvas.enabled = true;
    }
}
