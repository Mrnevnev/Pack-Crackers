using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    public static EnemyController instance;

    public void Awake()
    {
        instance = this;
    }

    public List<CardScriptableObject> deckToUse = new List<CardScriptableObject>();
    private List<CardScriptableObject> activeCards = new List<CardScriptableObject>();

    public Card cardToSpawn;
    public Transform cardSpawnPoint;
    
    public enum AIType
    {
        placedFromDeck, handRandomPlace, handDefensive, handAttacking
    }

    public AIType enemyAiType;

    private List<CardScriptableObject> cardsInHand = new List<CardScriptableObject>();
    public int startHandSize;
    

    // Start is called before the first frame update
    void Start()
    {
        SetUpDeck();

        if (enemyAiType != AIType.placedFromDeck)
        {
            SetUpHand();
        }
        
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // Enemy deck setup 
    public void SetUpDeck()
    {
        activeCards.Clear();

        List<CardScriptableObject> tempDeck = new List<CardScriptableObject>();
        tempDeck.AddRange(deckToUse);

        int interations = 0;

        while (tempDeck.Count > 0 && interations < 500)
        {
            int selected = Random.Range(0, tempDeck.Count);

            activeCards.Add(tempDeck[selected]);
            tempDeck.RemoveAt(selected);

            interations++;
        }


    }

    public void StartAction()
    {
        StartCoroutine(EnemyActionCo());
    }

        
    //Enemy AI
    IEnumerator EnemyActionCo()
    {

        if (activeCards.Count == 0)
        {
            SetUpDeck();
        }
        yield return new WaitForSeconds(0.5f);

        if (enemyAiType != AIType.placedFromDeck)
        {
            for (int i = 0; i < BattleController.instance.cardsToDrawPerTurn; i++)
            {
                cardsInHand.Add(activeCards[0]);
                activeCards.RemoveAt(0);

                if (activeCards.Count == 0)
                    
                {
                    SetUpDeck();
                }

            }
        }

        List<CardPlacePoint> cardPoints = new List<CardPlacePoint>();
        
        cardPoints.AddRange(CardsPointsController.instance.enemyCardPoints);
        
        //picking a random point on the board
        int randomPoint = Random.Range(0, cardPoints.Count);
        CardPlacePoint selectedPoint = cardPoints[randomPoint];

        if (enemyAiType == AIType.placedFromDeck || enemyAiType == AIType.handRandomPlace)
        {
            cardPoints.RemoveAt(randomPoint);
            
            
            //Goes through the points checking if there is a card there to place at random.
            while (selectedPoint.activeCard != null && cardPoints.Count > 0)
            {
                randomPoint = Random.Range(0, cardPoints.Count);
                selectedPoint = cardPoints[randomPoint];
                cardPoints.RemoveAt(randomPoint);
            }
        }

        CardScriptableObject selectedCard = null;
        int iterations = 0;

        List<CardPlacePoint> preferredPoints = new List<CardPlacePoint>();
        List<CardPlacePoint> secondaryPoints = new List<CardPlacePoint>();
        

        switch (enemyAiType)
        {
            
            case AIType.placedFromDeck:
                if (selectedPoint.activeCard == null)

                {
                    Card newCard = Instantiate(cardToSpawn, cardSpawnPoint.position, cardSpawnPoint.rotation);
                
                    newCard.cardSObject = activeCards[0];
                    activeCards.RemoveAt(0);
                    newCard.SetupCard();
                    newCard.MoveToPoint(selectedPoint.transform.position, selectedPoint.transform.rotation);
                
                    selectedPoint.activeCard = newCard;
                    newCard.assignedPlace = selectedPoint;
                    
                }
                break;
            
            case AIType.handAttacking:
                
                   selectedCard = SelectedCardToPlay();
                
                preferredPoints.Clear();
                secondaryPoints.Clear();

                for (int i = 0; i < cardPoints.Count; i++)
                {
                    if (cardPoints[i].activeCard == null)
                    {
                        if (CardsPointsController.instance.playerCardPoints[i].activeCard == null)
                        {
                            preferredPoints.Add(cardPoints[i]);
                            
                        }
                        else
                        {
                            secondaryPoints.Add(cardPoints[i]);
                        }
                    }
                }
                //checking to see if slot is available 
                
                //can always add more information i.e block cards with x amount of health
                iterations = 50;
                while (selectedCard != null && iterations > 0 && preferredPoints.Count + secondaryPoints.Count > 0)
                {
                    if (preferredPoints.Count > 0)
                    {
                        int selectPoint = Random.Range(0, preferredPoints.Count);
                        selectedPoint = preferredPoints[selectPoint];
                        
                        preferredPoints.RemoveAt(selectPoint);
                    }
                    else
                    {
                        int selectPoint = Random.Range(0, secondaryPoints.Count);
                        selectedPoint = secondaryPoints[selectPoint];
                        
                        secondaryPoints.RemoveAt(selectPoint);
                    }
                    
                    PlayCard(selectedCard, selectedPoint);
                    
                    // check if we should play another card

                    selectedCard = SelectedCardToPlay();

                    iterations--;
                    
                    yield return new WaitForSeconds(CardsPointsController.instance.timeBetweenAttacks);
                }
                
                break;
            
            case AIType.handDefensive:

                selectedCard = SelectedCardToPlay();
                
                preferredPoints.Clear();
                secondaryPoints.Clear();

                for (int i = 0; i < cardPoints.Count; i++)
                {
                    if (cardPoints[i].activeCard == null)
                    {
                        if (CardsPointsController.instance.playerCardPoints[i].activeCard != null)
                        {
                            preferredPoints.Add(cardPoints[i]);
                            
                        }
                        else
                        {
                            secondaryPoints.Add(cardPoints[i]);
                        }
                    }
                }
                //checking to see if slot is available 
                
                //can always add more information i.e block cards with x amount of health
                iterations = 50;
                while (selectedCard != null && iterations > 0 && preferredPoints.Count + secondaryPoints.Count > 0)
                {
                    if (preferredPoints.Count > 0)
                    {
                        int selectPoint = Random.Range(0, preferredPoints.Count);
                        selectedPoint = preferredPoints[selectPoint];
                        
                        preferredPoints.RemoveAt(selectPoint);
                    }
                    else
                    {
                        int selectPoint = Random.Range(0, secondaryPoints.Count);
                        selectedPoint = secondaryPoints[selectPoint];
                        
                        secondaryPoints.RemoveAt(selectPoint);
                    }
                    
                    PlayCard(selectedCard, selectedPoint);
                    
                    // check if we should play another card

                    selectedCard = SelectedCardToPlay();

                    iterations--;
                    
                    yield return new WaitForSeconds(CardsPointsController.instance.timeBetweenAttacks);
                }
                break;
            
            case AIType.handRandomPlace:

                selectedCard = SelectedCardToPlay();

                iterations = 50;

                while (selectedCard != null && iterations > 0 && selectedPoint.activeCard == null)
                {
                    PlayCard(selectedCard, selectedPoint);

                    selectedCard = SelectedCardToPlay();

                    iterations--;

                    yield return new WaitForSeconds(CardsPointsController.instance.timeBetweenAttacks);
                    
                    while (selectedPoint.activeCard != null && cardPoints.Count > 0)
                    {
                        randomPoint = Random.Range(0, cardPoints.Count);
                        selectedPoint = cardPoints[randomPoint];
                        cardPoints.RemoveAt(randomPoint);
                    }
                }

                break;
        }
        
        //Time between enemy putting their card down and the card attacking.
        yield return new WaitForSeconds(0.5f);
        
        BattleController.instance.AdvanceTurn();
    }

    void SetUpHand()
    {
        for (int i = 0; i < startHandSize; i++)
        {

            if (activeCards.Count == 0)
            {
                SetUpDeck();
            }
            cardsInHand.Add(activeCards[0]);
            activeCards.RemoveAt(0);
            
        }
    }

    public void PlayCard(CardScriptableObject cardSO, CardPlacePoint placedPoint)
    {
        Card newCard = Instantiate(cardToSpawn, cardSpawnPoint.position, cardSpawnPoint.rotation);
                
        newCard.cardSObject = cardSO;

        newCard.SetupCard();
        newCard.MoveToPoint(placedPoint.transform.position, placedPoint.transform.rotation);
                
        placedPoint.activeCard = newCard;
        newCard.assignedPlace = placedPoint;

        cardsInHand.Remove(cardSO);
        
        BattleController.instance.SpendEnemyGold(cardSO.cardCost);
    }
    
    
    // How the AI picks the cards
    CardScriptableObject SelectedCardToPlay()
    {
        CardScriptableObject cardToPlay = null;

        List<CardScriptableObject> cardsToPlay = new List<CardScriptableObject>();
        //cardToPlay = cardsToPlay[Random.Range(0, cardsToPlay.Count)];
        foreach (CardScriptableObject card in cardsInHand)
        {
            if (card.cardCost <= BattleController.instance.enemyGold)
            {
                cardsToPlay.Add(card);
            }
        }

        if (cardsToPlay.Count > 0)
        {
            int selected = Random.Range(0, cardsToPlay.Count);

            cardToPlay = cardsToPlay[selected];
        }
        


        return cardToPlay;
    }
        

}


