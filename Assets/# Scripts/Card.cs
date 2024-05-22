using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    
    public CardScriptableObject cardSObject;

    public bool isPlayer;
    
    public int cardHealth, cardAttack, cardCost;

   public TMP_Text healthText, attackText, costText, nameText, abilityText, loreText;

   public Image cardSprite, cardBackground;

    private Vector3 targetPoint;
    private Quaternion targetRotation;
    
    private float moveSpeed = 5f;
    private float rotationSpeed = 540f;

     public bool inHand;

     public int handPosition;

    private HandController theHC;

    private bool isSelected;
    private Collider theCollider;

    [SerializeField] private LayerMask whatIsDesktop, whatIsPlacement;

    [SerializeField] private bool justPressed;

    public CardPlacePoint assignedPlace;

    public Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        if (targetPoint == Vector3.zero)
        {
            targetPoint = transform.position;
            targetRotation = transform.rotation;
        }
        SetupCard();

        theHC = FindObjectOfType<HandController>();
        theCollider = GetComponent<Collider>();

    }
    
    public void SetupCard()
    {
        cardHealth = cardSObject.cardHealth;
        cardAttack = cardSObject.cardAttack;
        cardCost = cardSObject.cardCost;
        
       /* healthText.text = cardHealth.ToString();
        attackText.text = cardAttack.ToString();
        costText.text = cardCost.ToString();
        */
        
       UpdateCardDisplay();
       
        nameText.text = cardSObject.cardName;
        abilityText.text = cardSObject.cardAbility;
        loreText.text = cardSObject.cardLore;
        
        //cardSprite.sprite = cardSObject.characterSprite;//
        cardBackground.sprite = cardSObject.characterImage;
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPoint, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (isSelected && BattleController.instance.battleEnded == false && Time.timeScale != 0f)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, whatIsDesktop))
            {
               MoveToPoint(hit.point + new Vector3(0f, 2f, 0f), quaternion.identity);
            }

            if (Input.GetMouseButtonDown(1) && BattleController.instance.battleEnded == false)
            {
                ReturnToHand();
            }
            
            if (Input.GetMouseButtonDown(0) && justPressed == false  && BattleController.instance.battleEnded == false )
            {   
                //Limiting when the player can use a card.
                if (Physics.Raycast(ray, out hit, 50f, whatIsPlacement) && BattleController.instance.currentPhase == BattleController.TurnOrder.PlayerActive)
                {
                    CardPlacePoint selectedPoint = hit.collider.GetComponent<CardPlacePoint>();
                    
                    if (selectedPoint.activeCard == null && selectedPoint.isPlayPoint)
                    {
                        if (BattleController.instance.currentGold >= cardCost)
                        {
                            selectedPoint.activeCard = this;
                            assignedPlace = selectedPoint;
                            MoveToPoint(selectedPoint.transform.position, quaternion.identity);

                            inHand = false;
                            isSelected = false;
                        
                            theHC.RemoveCardFromHand(this);
                            
                            BattleController.instance.SpendPlayerGold(cardCost);
                            
                            AudioManager.instance.PlaySFX(4);
                            
                        }
                        else
                        {
                            ReturnToHand();
                            
                            UiController.instance.ShowGoldWarning();
                        }
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
        }

        justPressed = false;
    }
    
    public void MoveToPoint(Vector3 pointToMoveTo, Quaternion rotationToMatch)
    {
        targetPoint = pointToMoveTo;
        targetRotation = rotationToMatch;
        // Code to move the card to the target point
    }

    private void OnMouseOver()
    {
        //Limiting the player from using a when the turn is over and they are still holding the card.
        if (inHand && BattleController.instance.currentPhase == BattleController.TurnOrder.PlayerActive && isPlayer && BattleController.instance.battleEnded == false )
        {
            MoveToPoint(theHC.cardPosition[handPosition] + new Vector3(0f, 1f, 0.5f),quaternion.identity);
        }
    }

    private void OnMouseExit()
    {
        if (inHand && isPlayer && BattleController.instance.battleEnded == false )
        {
            MoveToPoint(theHC.cardPosition[handPosition],theHC.minPosition.rotation);
        }
    }

    private void OnMouseDown()
    {
        if (inHand && isPlayer && BattleController.instance.battleEnded == false && Time.timeScale != 0f)
        {
            isSelected = true;
            theCollider.enabled = false;
            justPressed = true;
            assignedPlace = null;
        }
    }

    public void ReturnToHand()
    {
        isSelected = false;
        theCollider.enabled = true;
        
        MoveToPoint(theHC.cardPosition[handPosition], theHC.minPosition.rotation);
        
        //theHC.returnCardToHand(this);
    }

    public void DamageCard(int damageAmount)
    {
        cardHealth -= damageAmount;
        if (cardHealth <= 0)
        {
            cardHealth = 0;

            assignedPlace.activeCard = null;
            
            
            // Used for moving cards off screen to discard pile.
            MoveToPoint(BattleController.instance.discardPoint.position, BattleController.instance.discardPoint.rotation);
            
            //Death animation.
            anim.SetTrigger("Jump");
            
            Destroy(gameObject, 2f);
            
            AudioManager.instance.PlaySFX(2); //Card Died
            
        }
        else
        {
            AudioManager.instance.PlaySFX(1); //Card Attack
        }
        
        anim.SetTrigger("Hurt");
        
        UpdateCardDisplay();
    }

    public void UpdateCardDisplay()
    {
        healthText.text = cardHealth.ToString();
        attackText.text = cardAttack.ToString();
        costText.text = cardCost.ToString();
    }
}

