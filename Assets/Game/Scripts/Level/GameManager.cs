using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool _isPaused = false;

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
        OnInit();
    }

    private void OnInit()
    {
        UIManager.Instance.MainMenuInit();
        UIManager.Instance.OnInit();
    }

    //public void PauseGame()
    //{
    //    UIManager.Instance.TogglePauseCanvas();
    //    _isPaused = !_isPaused;
    //    if (_isPaused)
    //    {
    //        Time.timeScale = 0f;
    //    }
    //    else
    //    {
    //        Time.timeScale = 1;
    //    }
    //}


    public void StartNewGame()
    {
        Room currentRoom = DungeonTraversalManager.Instance.GetCurrentRoom();
        if (currentRoom != null)
        {
            Destroy(currentRoom.gameObject);
        }
        DungeonManager.Instance.OnInit();
        LevelManager.Instance.OnInit();
        SpawnManager.Instance.OnInit();
        UIManager.Instance.OnInit();
        SpawnManager.Instance.SpawnPlayer();
        CoinManager.Instance.OnInit();
        PlayerEquipmentManager.Instance.OnInit();
        _isPaused = false;
    }
}
