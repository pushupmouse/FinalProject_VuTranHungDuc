using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _quitButton;

    private void Start()
    {
        _startButton.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        UIManager.Instance.StartGame();
        GameManager.Instance.StartNewGame();
    }
}
