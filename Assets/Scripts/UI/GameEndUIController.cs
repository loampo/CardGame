using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEndUIController : MonoBehaviour
{
    public TextMeshProUGUI winnerName;
    public Button restart, quit;

    private void Awake()
    {
        SetupButtons();
    }

    private void SetupButtons()
    {
        restart.onClick.AddListener(() =>
        {
            RestartMatch();
        });

        quit.onClick.AddListener(() =>
        {
            QuitGame();
        });
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void RestartMatch()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Initialize(Player winner)
    {
        winnerName.text = "Vincitore Player " + (winner.ID + 1);
    }
}
