using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreenManager : MonoBehaviour
{
    [SerializeField] private Button _newGameButton;

    private void Start()
    {
        _newGameButton.onClick.AddListener(StartNewGame);
    }

    private void StartNewGame()
    {
        GameManager.Instance.StartNewGame();
    }
}
