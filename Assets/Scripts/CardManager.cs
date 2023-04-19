using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;    
    public List<Cards> cards = new List<Cards>(); // List of all cards in the game
    public Transform player1Hand, player2Hand;
    public CardController cardControllerPrefab;
    // Lists to keep track of the cards for each player
    public List<CardController> player1Cards = new List<CardController>(), player2Cards = new List<CardController>(),
        player1HandCards = new List<CardController>(), player2HandCards = new List<CardController>();
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GenerateCards();
    }

   
    private void GenerateCards()
    {
        // Loop through all cards and add them to player 1's hand
        foreach (Cards card in cards)
        {
            CardController newCard = Instantiate(cardControllerPrefab, player1Hand);
            newCard.transform.localPosition = Vector3.zero;
            newCard.Initialize(card, 0);
            player1HandCards.Add(newCard);
        }
        // Loop through all cards and add them to player 2's hand
        foreach (Cards card in cards)
        {
            CardController newCard = Instantiate(cardControllerPrefab, player2Hand);
            newCard.transform.localPosition = Vector3.zero;
            newCard.Initialize(card, 1);
            player2HandCards.Add(newCard);
        }
    }
    
    public void PlayCard(CardController card, int ID) // Called when a card is played
    {
        if (ID == 0)
        {
            player1Cards.Add(card);
            player1HandCards.Remove(card);
        }
        else
        {
            player2Cards.Add(card);
            player2HandCards.Remove(card);
        }
    }
    
    public void ProcessStartTurn(int ID) // Called at the start of a player's turn
    {
        // Create lists of the current player's cards and the enemy player's cards
        List<CardController> cards = new List<CardController>();
        List<CardController> enemyCard = new List<CardController>();

        if (ID == 0)
        {
            cards.AddRange(player1Cards);
            enemyCard.AddRange(player2Cards);
        }
        else
        {
            cards.AddRange(player2Cards);
            enemyCard.AddRange(player1Cards);
        }
        foreach (CardController card in cards)
        {
            if (card == null) continue;
            if (card.card.health <= 0)
            {
                Destroy(card.gameObject);
            }
        }
        
        foreach (CardController card in enemyCard)// Loop through all cards in the current player's hand and destroy any with 0 health
        {
            if (card.card.health <= 0)
            {
                Destroy(card.gameObject);
            }
        }
        // Clear the player's cards for the start of the turn
        player1Cards.Clear();
        player2Cards.Clear();

        foreach (CardController card in cards)// Loop through all cards in the enemy player's hand and destroy any with 0 health
        {
            if (card != null)
            {
                if (ID == 0)
                {
                    player1Cards.Add(card);
                }
                else
                {
                    player2Cards.Add(card);
                }
            }

        }
        foreach (CardController card in enemyCard)// Add cards to the player's hand that are still in play
        {
            if (card != null)
            {
                if (ID == 1)
                {
                    player1Cards.Add(card);
                }
                else
                {
                    player2Cards.Add(card);
                }
            }

        }
        bool drawCard = false;
        if (ID == 0)
        {
            drawCard = player1HandCards.Count < 7;
        }else
        {
            drawCard = player2HandCards.Count < 7;
        }
        if (drawCard)
        {
            int randomCard = UnityEngine.Random.Range(0, this.cards.Count);
            CardController newCard = Instantiate(cardControllerPrefab, ID == 0 ? player1Hand : player2Hand);
            newCard.transform.localPosition = Vector3.zero;
            newCard.Initialize(this.cards[randomCard], ID);
            if (ID == 0)
                player1HandCards.Add(newCard);
            else
                player2HandCards.Add(newCard);
        }
    }

    public void ProcessEndTurn(int ID)
    {
        // Create two lists to hold the player's cards and the enemy's cards
        List<CardController> cards = new List<CardController>();
        List<CardController> enemyCards = new List<CardController>();
        if (ID == 0) // Add the player's cards and the enemy's cards to the respective lists
        {
            cards.AddRange(player1Cards);
            enemyCards.AddRange(player2Cards);
        }
        else
        {
            cards.AddRange(player2Cards);
            enemyCards.AddRange(player1Cards);
        }

        foreach (CardController cardController in cards)
        {
            if (CardsInArea(enemyCards)) // Check if there are any enemy cards in the play area
            {
                // If there are, select a random enemy card that has health remaining
                int randomEnemyCard = UnityEngine.Random.Range(0, enemyCards.Count);
                while (enemyCards[randomEnemyCard].card.health <= 0)
                {
                    randomEnemyCard = UnityEngine.Random.Range(0, enemyCards.Count);
                }
                // Apply damage to both the enemy card and the player's card
                enemyCards[randomEnemyCard].Damage(cardController.card.damage);
                cardController.Damage(enemyCards[randomEnemyCard].card.damage);
            }
            else
            {
                // If there are no enemy cards, damage the enemy player directly
                int enemyID = ID == 0 ? 1 : 0;
                PlayerManager.instance.DamagePlayer(enemyID,cardController.card.damage);
            }
        }
    }


    private bool CardsInArea(List<CardController> cards) // This function checks if there are any cards in the play area that have health remaining
    {
        bool cardWithHealth = false;
        foreach (CardController card in cards)
        {
            if(card.card.health > 0)
            {
                cardWithHealth = true;
            }
        }
        return cardWithHealth;
    }
}
