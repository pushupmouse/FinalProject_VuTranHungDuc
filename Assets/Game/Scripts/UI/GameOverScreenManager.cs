using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreenManager : MonoBehaviour
{
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _mainMenuButton;

    private void Start()
    {
        _newGameButton.onClick.AddListener(StartNewGame);
    }

    private void StartNewGame()
    {
        GameManager.Instance.StartNewGame();
    }
}
