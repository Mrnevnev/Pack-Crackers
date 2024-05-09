using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeckController : MonoBehaviour
{

    public static DeckController instance;

    private void Awake()
    {
        instance = this;
    }

    public List<CardScriptableObject> deckToUse = new List<CardScriptableObject>();

    private List<CardScriptableObject> activeCards = new List<CardScriptableObject>();

    public Card cardToSpawn;

    public int drawCardCost = 2;

    public float waitBetweenDrawingCards = .25f;

    // Start is called before the first frame update
    void Start()
    {
        SetUpDeck();
    }

    // Update is called once per frame
    void Update()
    {
        /* if (Input.GetKeyDown(KeyCode.T))
         {
             DrawCardToHand();
         }*/
    }

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

    public void DrawCardToHand()
    {
        if (activeCards.Count == 0)
        {
            SetUpDeck();
        }

        Card newCard = Instantiate(cardToSpawn, transform.position, transform.rotation);
        newCard.cardSObject = activeCards[0];
        newCard.SetupCard();
        activeCards.RemoveAt(0);
        HandController.instance.AddCardToHand(newCard);
    }

    public void DrawCardForGold()
    {
        if (BattleController.instance.currentGold >= drawCardCost)
        {
            
            DrawCardToHand();
            BattleController.instance.SpendPlayerGold(drawCardCost);
            BattleController.instance.SpendEnemyGold(drawCardCost);

        }
        else
        {
            UiController.instance.ShowGoldWarning();
            UiController.instance.drawCardButton.SetActive(false);
        }
    }

    public void DrawMultipleCards(int amountToDraw)
    {
        StartCoroutine(DrawMultipleCo(amountToDraw));
    }

    IEnumerator DrawMultipleCo(int amountToDraw)
    {
        for (int i = 0; i < amountToDraw; i++)
        {
            DrawCardToHand();
            
            yield return new WaitForSeconds(waitBetweenDrawingCards);
        }
    }

}