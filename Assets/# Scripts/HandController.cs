using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{

    public static HandController instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] public List<Card> heldCards = new List<Card>();

    [SerializeField] public Transform minPosition, maxPosition;

    [SerializeField] public List<Vector3> cardPosition = new List<Vector3>();



    // Start is called before the first frame update
    void Start()
    {
        SetCardPositionInHand();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetCardPositionInHand()
    {
        cardPosition.Clear();

        Vector3 distanceBetweenPoints = Vector3.zero;
        if (heldCards.Count > 1)
        {
            distanceBetweenPoints = (maxPosition.position - minPosition.position) / (heldCards.Count - 1);
        }

        for (int i = 0; i < heldCards.Count; i++)
        {
            cardPosition.Add(minPosition.position + (distanceBetweenPoints * i));

            // heldCards[i].transform.position = cardPosition[i];
            // heldCards[i].transform.rotation = minPosition.rotation;

            heldCards[i].MoveToPoint(cardPosition[i], minPosition.rotation);
            heldCards[i].handPosition = i;
            heldCards[i].inHand = true;

        }

    }

    public void RemoveCardFromHand(Card cardToRemove)
    {
        if (heldCards[cardToRemove.handPosition] == cardToRemove)
        {
            heldCards.RemoveAt(cardToRemove.handPosition);
        }
        else
        {
            Debug.Log("Card at postion " + cardToRemove.handPosition + "is not the right card.");
        }

        SetCardPositionInHand();
    }

    public void AddCardToHand(Card cardToAdd)
    {
        if (cardToAdd != null)
        {
            heldCards.Add(cardToAdd);
            SetCardPositionInHand();
        }
        else
        {
            Debug.LogError("Trying to add a null card to hand");
        }
    }

    public void EmptyHand()
    {
        foreach (Card heldCard in heldCards)
        {
            heldCard.inHand = false;
            heldCard.MoveToPoint(BattleController.instance.discardPoint.position, heldCard.transform.rotation);
        }
        
        heldCards.Clear();
    }
    
}