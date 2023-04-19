using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public List<Player> playerList = new List<Player>();// A list of the players in the game

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UIManager.instance.UpdateValues(playerList[0], playerList[1]);
    }
    /// <summary>
    /// Set the 'myTurn' flag to true for the current player and add 1 mana each turn
    /// </summary>
    /// <param></param>
    internal void AssignTurn(int currentPlayerTurn)
    {
        foreach (Player player in playerList)
        {
            player.myTurn = player.ID == currentPlayerTurn;
            if (player.myTurn) player.mana += 1;
        }
        UIManager.instance.UpdateManaValues(playerList[0].mana, playerList[1].mana);
    }

    public void DamagePlayer(int ID, int damage)
    {// Decrease the player's health by the given amount
        Player player = FindPlayerByID(ID);
        FindPlayerByID(ID).health -= damage;
        UIManager.instance.UpdateHealthValues(playerList[0].health, playerList[1].health);
        if (player.health <= 0)
        {// If the player's health is less than or equal to 0, they lost the game
            PlayerLost(ID);
        }
    }

    private void PlayerLost(int ID)
    {
        UIManager.instance.GameFinished(ID == 0 ? FindPlayerByID(1) : FindPlayerByID(0));
    }

    public Player FindPlayerByID(int ID)
    {
        Player foundPlayer = null;
        foreach (Player player in playerList)
        {
            if (player.ID == ID) foundPlayer = player;
        } return foundPlayer;
    }

    internal void SpendMana(int ownerID, int cost)
    {// Decrease the current player's mana by the cost of the card played
        FindPlayerByID(ownerID).mana -= cost;
        UIManager.instance.UpdateManaValues(playerList[0].mana, playerList[1].mana);
    }
}
