using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
 
    [SerializeField] private int _currentLevel = 1;
    [SerializeField] private List<LevelData> _levelDatas;

    public int CurrentLevel => _currentLevel;

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
        _currentLevel = 1;
    }

    public void IncreaseLevel()
    {
        if(_currentLevel < _levelDatas.Count)
        {
            _currentLevel++;
        }
        SpawnManager.Instance.OnInit();
        DungeonTraversalManager.Instance.RecreateDungeon();
        ShopManager.Instance.OnInit();
    }

    public LevelData GetLevelData()
    {
        return _levelDatas[_currentLevel - 1];
    }
}
