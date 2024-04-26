using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
 
    [SerializeField] private int _currentLevel;
    [SerializeField] private List<LevelData> _levelDatas;

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
        _currentLevel = 1;
        SpawnManager.Instance.OnInit();
    }

    public void IncreaseLevel()
    {
        if(_currentLevel < _levelDatas.Count)
        {
            _currentLevel++;
        }
        SpawnManager.Instance.OnInit();
        DungeonTraversalManager.Instance.MoveToNewLevel();
        ShopManager.Instance.OnInit();
    }

    public LevelData GetLevelData()
    {
        return _levelDatas[_currentLevel - 1];
    }
}
