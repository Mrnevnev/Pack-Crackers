// UiController 
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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

    public string mainMenuScene, optionScene, battleSelectScene;

    public GameObject pauseScreen;
    
    
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
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
        
        AudioManager.instance.PlaySFX(0);
    }

    public void EndPlayerTurn()
    {
        BattleController.instance.EndPlayerTurn();
        
        AudioManager.instance.PlaySFX(0);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
        Time.timeScale = 1f;
        
        AudioManager.instance.PlaySFX(0);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
        
        AudioManager.instance.PlaySFX(0);
    }

    public void SelectEnemy()
    {
        SceneManager.LoadScene(battleSelectScene);
        Time.timeScale = 1f;
        
        AudioManager.instance.PlaySFX(0);
    }
    
    public void Option()
    {
        SceneManager.LoadScene(optionScene);
        Time.timeScale = 1f;
        
        AudioManager.instance.PlaySFX(0);
    }

    public void PauseUnpause()
    {
        if (pauseScreen.activeSelf == false)
        {
            pauseScreen.SetActive(true);

            Time.timeScale = 0f;
            
            AudioManager.instance.PlaySFX(0);
        }
        else
        {
            pauseScreen.SetActive(false);
            
            Time.timeScale = 1f;
            
            AudioManager.instance.PlaySFX(0);
        }
    }
    
}
