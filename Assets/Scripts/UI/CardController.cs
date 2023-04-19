using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class CardController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Cards card;
    public Image illustration, image;
    public TextMeshProUGUI cardName, health, cost, damage;
    private Transform originalParent;
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    // Initializes the card with its properties and sets its owner ID
    public void Initialize(Cards card, int ownerID) 
    {
        this.card = new Cards(card);
        this.card.ownerID = ownerID;
        illustration.sprite = card.illustration;
        cardName.text = card.cardName;
        cost.text = card.cost.ToString();
        damage.text = card.damage.ToString();
        health.text = card.health.ToString();
        originalParent = transform.parent;
        if (card.health == 0) health.text = "";
    }
    // Reduces the health of the card by a certain amount and updates the displayed health value
    public void Damage(int amount)
    {
        card.health -= amount;
        health.text = card.health.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
    // Checks if the card is in the current player's area and can be played
    public void OnPointerDown(PointerEventData eventData)
    {
        if(originalParent.name == $"Player{card.ownerID + 1}Area" || TurnManager.instance.currentPlayerTurn != card.ownerID)
        {

        }
        else
        {
            // Detaches the card from its parent and disables raycasting to allow dragging
            transform.SetParent(transform.root);
            image.raycastTarget = false;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Checks if the card is in the current player's area and can be played
        if (originalParent.name == $"Player{card.ownerID + 1}Area" || TurnManager.instance.currentPlayerTurn != card.ownerID)
        {

        }
        else// Reenables raycasting and analyzes the pointer up event
        {
            image.raycastTarget = true;
            AnalyzePointerUp(eventData);
        }
        
    }
    // Analyzes the pointer up event and determines if the card should be played or returned to the hand
    private void AnalyzePointerUp(PointerEventData eventData)
    {
        if(eventData.pointerEnter != null && eventData.pointerEnter.name == $"Player{card.ownerID+1}Area")
        {
            if (PlayerManager.instance.FindPlayerByID(card.ownerID).mana >= card.cost)
            {
                PlayCard(eventData.pointerEnter.transform);
                PlayerManager.instance.SpendMana(card.ownerID,card.cost);
            }
            else
            {
                ReturnToHand();
            }
        }
        else
        {
            ReturnToHand();
        }
    }

    // Plays the card by setting its parent to the play area, resetting its position, and calling CardManager to handle the play
    private void PlayCard(Transform playArea)
    {
        transform.SetParent(playArea);
        transform.localPosition = Vector3.zero;
        originalParent = playArea;
        CardManager.instance.PlayCard(this, card.ownerID);
    }

    // Returns the card to the hand
    private void ReturnToHand()
    {
        transform.SetParent(originalParent);
        transform.localPosition = Vector3.zero;
    }

    // Handles when the card is being dragged
    public void OnDrag(PointerEventData eventData)
    {
        if (transform.parent == originalParent) return;
        transform.position = eventData.position;
    }
}
