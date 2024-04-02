using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    [SerializeField] private int currentCoins = 0;
    public int CurrentCoins => currentCoins;

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

    public void AddCoins(int amount)
    {
        currentCoins += amount;
    }

    public void SubtractCoins(int amount)
    {
        if(currentCoins < amount)
        {
            return;
        }

        currentCoins -= amount;
    }
}
