// UiController 
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiController : MonoBehaviour
{
    public static UiController instance;

    public void Awake()
    {
        instance = this;

    }

    public TMP_Text playerGoldText, playerHealthText, enemyHealthText, enemyGoldText;
    public GameObject goldWarning;
    public float goldWarningTime;
    private float goldWarningCounter;

    public GameObject drawCardButton, endTurnButton;

    public UiDamageIndicator playerDamage, enemyDamage;

    public GameObject battleEndScreen;

    public TMP_Text battleResultText;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (goldWarningCounter > 0)
        {
            goldWarningCounter -= Time.deltaTime;
            if (goldWarningCounter <= 0)
            {
                goldWarning.SetActive(false);
            }
        }
    }

    public void SetPlayerGold(int goldAmount)
    {
        playerGoldText.text = "Gold: " + goldAmount;
    }

    public void SetEnemyGold(int goldAmount)
    {
        enemyGoldText.text = "Gold: " + goldAmount;
    }
    
    
    
    public void SetPlayerHealthText(int healthAmount)
    {
        playerHealthText.text = "Player Health: " + healthAmount;
    }
    
    public void SetEnemyHealthText(int healthAmount)
    {
        enemyHealthText.text = "Enemy Health: " + healthAmount;
    }

    public void ShowGoldWarning()
    {
        goldWarning.SetActive(true);
        goldWarningCounter = goldWarningTime;
    }

    public void DrawCard()
    {
        DeckController.instance.DrawCardForGold();
    }

    public void EndPlayerTurn()
    {
        BattleController.instance.EndPlayerTurn();
    }

    public void MainMenu()
    {
        
    }

    public void PlayAgain()
    {
        
    }

    public void SelectEnemy()
    {
        
    }
    
}
