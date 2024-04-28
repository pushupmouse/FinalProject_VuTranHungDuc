using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreenManager : MonoBehaviour
{
    public static GameOverScreenManager Instance;

    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private TextMeshProUGUI _text;

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
        _newGameButton.onClick.AddListener(StartNewGame);
        _mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    private void StartNewGame()
    {
        GameManager.Instance.StartNewGame();
    }

    private void GoToMainMenu()
    {
        UIManager.Instance.MainMenuInit();
    }

    public void ChangeToVictory()
    {
        _text.text = "Dungeon Cleared!";
    }

    public void ChangeToDefeat()
    {
        _text.text = "You Died.";
    }
}
