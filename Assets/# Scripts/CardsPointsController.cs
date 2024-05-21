using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsPointsController : MonoBehaviour
{
    public static CardsPointsController instance;
    public float timeBetweenAttacks = .5f;

    private void Awake()
    {
        instance = this;
    }

    public CardPlacePoint[] playerCardPoints, enemyCardPoints;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerAttack()
    {
        StartCoroutine(PlayerAttackCo());
       /* foreach (CardPlacePoint point in playerCardPoints)
        {
            if (point.activeCard != null && point.isPlayPoint)
            {
                point.activeCard.Attack();
            }
        } */
    }

    IEnumerator PlayerAttackCo()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        
        for (int i = 0; i < playerCardPoints.Length; i++)
            
        {
            if (playerCardPoints[i].activeCard != null)
            {
                if (enemyCardPoints[i].activeCard != null)
                {
                    //Attacking the enemy's cards
                    enemyCardPoints[i].activeCard.DamageCard(playerCardPoints[i].activeCard.cardAttack);
                    

                }
                else
                {
                    //attack enemy's health.
                    BattleController.instance.DamageEnemy(playerCardPoints[i].activeCard.cardAttack);
                }
                playerCardPoints[i].activeCard.anim.SetTrigger("Attack");

                yield return new WaitForSeconds(timeBetweenAttacks);
            }
            
            //stops the rest of the cards from attacking after life is 0
            if (BattleController.instance.battleEnded == true)
            {
                i = playerCardPoints.Length;
            }
        }
        CheckAssignedCards();
        BattleController.instance.AdvanceTurn();
    }
    
    public void EnemyAttack()
    {
        StartCoroutine(EnemyAttackCo());
    }
    IEnumerator EnemyAttackCo()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        for (int i = 0; i < enemyCardPoints.Length; i++)
        {
            if (enemyCardPoints[i].activeCard != null)
            {
                if (playerCardPoints[i].activeCard != null)
                {
                    //Attacking the player's cards
                    playerCardPoints[i].activeCard.DamageCard(enemyCardPoints[i].activeCard.cardAttack);
                    
                }
                else
                {
                    //attack players's health.
                    BattleController.instance.DamagePlayer(enemyCardPoints[i].activeCard.cardAttack);
                }
                enemyCardPoints[i].activeCard.anim.SetTrigger("Attack");
                yield return new WaitForSeconds(timeBetweenAttacks);
            }
            
            //stops the rest of the cards from attacking after life is 0
            if (BattleController.instance.battleEnded == true)
            {
                i = enemyCardPoints.Length;
            }
        }
        CheckAssignedCards();
        BattleController.instance.AdvanceTurn();
    }

    public void CheckAssignedCards()
    {
        foreach (CardPlacePoint point in playerCardPoints)
        {
            if (point.activeCard != null /*&& point.isPlayPoint */ ) 
            {
                if (point.activeCard.cardHealth <= 0)

                {
                    point.activeCard = null;
                }
                
                /*point.activeCard.Attack();*/
            }
        }
        foreach (CardPlacePoint point in enemyCardPoints)
        {
            if (point.activeCard != null /*&& point.isPlayPoint */ ) 
            {
                if (point.activeCard.cardHealth <= 0)

                {
                    point.activeCard = null;
                }
                
                /*point.activeCard.Attack();*/
            }
        }
    }
}
