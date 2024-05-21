//CardScriptableObject
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card", order = 1)]
public class CardScriptableObject : ScriptableObject
{
    public string cardName;
    
    [TextArea]
    public string cardAbility, cardLore;

    public int cardHealth, cardAttack, cardCost;

    public Sprite /*characterSprite*/ characterImage;
    
    
}
