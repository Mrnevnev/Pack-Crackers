using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueMusic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlayBattleSelectMusic();
    }

    // Update is called once per frame
    void Update()
    {
       // AudioManager.instance.PlaySFX(0);
    }
}
