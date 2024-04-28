using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    [SerializeField] private int _currentCoins = 0;

    public int CurrentCoins => _currentCoins;

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

    public void OnInit()
    {
        _currentCoins = 0;
    }

    public void AddCoins(int amount)
    {
        _currentCoins += amount;

        InventoryManager.Instance.SetCoinText(_currentCoins);
    }

    public void SubtractCoins(int amount)
    {
        if(_currentCoins < amount)
        {
            return;
        }

        _currentCoins -= amount;

        InventoryManager.Instance.SetCoinText(_currentCoins);
    }
}
