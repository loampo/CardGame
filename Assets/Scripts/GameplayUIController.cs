using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GameplayUIController : MonoBehaviour
{
    public static GameplayUIController instance;
    public TextMeshProUGUI currentPlayerTurn;
    public Image imagePlayerTurn;
    public Button endTurn;

    private void Awake()
    {
        instance = this;
        SetupButtons();
    }

    private void SetupButtons()
    {
        endTurn.onClick.AddListener(() =>
        {
            TurnManager.instance.EndTurn();
        });
    }

    public void UpdateCurrentPlayerTurn(int ID)
    {        
        imagePlayerTurn.gameObject.SetActive(true);
        currentPlayerTurn.gameObject.SetActive(true);
        currentPlayerTurn.text = $"Player {ID + 1} Turn!";
        StartCoroutine(BlinkCurrentPlayerTurn());
    }

    private IEnumerator BlinkCurrentPlayerTurn()
    {
        yield return new WaitForSeconds(3);
        currentPlayerTurn.gameObject.SetActive(false);
        imagePlayerTurn.gameObject.SetActive(false);
    }
}
