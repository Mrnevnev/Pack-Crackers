using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleController : MonoBehaviour
{

    public static BattleController instance;

    public void Awake()
    {
        instance = this;
    }
    
    public int startingGold = 4, maxGold = 12;
    public int currentGold, enemyGold;
    
    
    private int currentPlayerMaxGold, currentEnemyMaxGold;
    

    public int startingCardAmount = 5;
    public int cardsToDrawPerTurn = 2;
    
    public enum TurnOrder { PlayerActive, PlayerCardAttacks, EnemyActive, EnemyCardAttacks }
    public TurnOrder currentPhase;
    public Transform discardPoint;

    public int playerHealth;
    public int enemyHealth;

    public bool battleEnded;

    public float resultScreenDelay = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
       // currentGold = startingGold;
        //UiController.instance.SetPlayerMana(currentGold);

        currentPlayerMaxGold = startingGold;
        currentEnemyMaxGold = startingGold;
        
        FillPlayerGold();
        FillEnemyGold();
        
        DeckController.instance.DrawMultipleCards(startingCardAmount);
        UiController.instance.SetEnemyHealthText(enemyHealth);
        UiController.instance.SetPlayerHealthText(playerHealth);
        

    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.T))
        {
            AdvanceTurn();
        }*/
    }

    public void SpendPlayerGold(int amountToSpend)
    {
        currentGold -= amountToSpend;
        if (currentGold < 0)
        {
            currentGold = 0;
        }
        UiController.instance.SetPlayerGold(currentGold);

    }
    
    public void FillPlayerGold()
    {
        // currentGold = startingGold;
        currentGold = currentPlayerMaxGold;
        UiController.instance.SetPlayerGold(currentGold);
    }
    
    
    public void SpendEnemyGold(int amountToSpend)
    {
        enemyGold -= amountToSpend;
        
        if (enemyGold < 0)
        {
            enemyGold = 0;
        }
        
        UiController.instance.SetEnemyGold(enemyGold);

    }
//4.4.2024
    public void FillEnemyGold()
    {
        enemyGold = currentEnemyMaxGold;
        UiController.instance.SetEnemyGold(enemyGold);
    }

    public void AdvanceTurn()
    {
        if (battleEnded == false)
        {


            currentPhase++;

            if ((int)currentPhase >= System.Enum.GetValues(typeof(TurnOrder)).Length)
            {
                currentPhase = 0;
            }

            // How turn order is picked.
            switch (currentPhase)
            {
                case TurnOrder.PlayerActive:

                    UiController.instance.endTurnButton.SetActive(true);
                    UiController.instance.drawCardButton.SetActive(true);

                    if (currentPlayerMaxGold < maxGold)
                    {
                        currentPlayerMaxGold++;
                    }

                    DeckController.instance.DrawMultipleCards(cardsToDrawPerTurn);

                    FillPlayerGold();

                    break;
                case TurnOrder.PlayerCardAttacks:
                    //Debug.Log("Skipping Player Attack");
                    //AdvanceTurn();

                    CardsPointsController.instance.PlayerAttack();


                    break;
                case TurnOrder.EnemyActive:

                    //Debug.Log("Skipping Enemy Turn");
                    //AdvanceTurn();

                    //Starting the enemy turn.


                    if (currentEnemyMaxGold < maxGold)
                    {
                        currentEnemyMaxGold++;
                    }

                    FillEnemyGold();

                    EnemyController.instance.StartAction();



                    break;
                case TurnOrder.EnemyCardAttacks:
                    // Debug.Log("Skipping Enemy Attack");
                    //AdvanceTurn();
                    CardsPointsController.instance.EnemyAttack();
                    break;
            }
        }

    }

    public void EndPlayerTurn()
    {
        UiController.instance.endTurnButton.SetActive(false);
        UiController.instance.drawCardButton.SetActive(false);
        AdvanceTurn();
    }

    public void DamagePlayer(int damageAmount)
    {
        if (playerHealth > 0 || battleEnded == false)
        {
            playerHealth -= damageAmount;
            if (playerHealth <= 0 )
            {
                playerHealth = 0;
                //ending battle
                
                EndBattle();
            }
            
            UiController.instance.SetPlayerHealthText(playerHealth);
            
            //Shows the damage that the player is taking
            UiDamageIndicator damageClone = Instantiate(UiController.instance.playerDamage, UiController.instance.playerDamage.transform.parent);
            damageClone.damageText.text = damageAmount.ToString();
            damageClone.gameObject.SetActive(true);
        }
    }

    public void DamageEnemy(int damageAmount)
    {
        if (enemyHealth > 0 || battleEnded == false)
        {
            enemyHealth -= damageAmount;
            if (enemyHealth <= 0)
            {
                enemyHealth = 0;
                //ending battle
                
                EndBattle();
            }
            
            UiController.instance.SetEnemyHealthText(enemyHealth);
            
            //Shows the damage that the player is taking
            UiDamageIndicator damageClone = Instantiate(UiController.instance.enemyDamage, UiController.instance.enemyDamage.transform.parent);
            damageClone.damageText.text = damageAmount.ToString();
            damageClone.gameObject.SetActive(true);
        }
       
    }

    void EndBattle()
    {
        battleEnded = true;
        
        HandController.instance.EmptyHand();

        if (enemyHealth <= 0)
        {
            UiController.instance.battleResultText.text = "YOU WON!";

            foreach (CardPlacePoint point in CardsPointsController.instance.enemyCardPoints)
            {
                if (point.activeCard != null)
                {
                    point.activeCard.MoveToPoint(discardPoint.position, point.activeCard.transform.rotation);
                }
            }
        }
        else
        {
            UiController.instance.battleResultText.text = "YOU LOST!";
            
            foreach (CardPlacePoint point in CardsPointsController.instance.playerCardPoints)
            {
                if (point.activeCard != null)
                {
                    point.activeCard.MoveToPoint(discardPoint.position, point.activeCard.transform.rotation);
                }
            }
        }

        StartCoroutine(ShowResultCo());
    }

    IEnumerator ShowResultCo()
    {

        yield return new WaitForSeconds(resultScreenDelay);
        
        UiController.instance.battleEndScreen.SetActive(true);
    }
    
}
