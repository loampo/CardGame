using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Cards
{
    public string cardName;
    public int health, damage, cost, ownerID;
    public Sprite illustration;

    public Cards()
    {

    }

    //Clone per poter differenziare carte nemiche 
    public Cards(Cards cards)
    {
        cardName = cards.cardName;
        health = cards.health;
        damage = cards.damage;
        cost = cards.cost;
        illustration = cards.illustration;
    }
}
