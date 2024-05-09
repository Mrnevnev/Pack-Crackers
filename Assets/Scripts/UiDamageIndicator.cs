using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiDamageIndicator : MonoBehaviour
{
    public TMP_Text damageText;

    public float moveSpeed;

    public float lifeTime = 3f;

    private RectTransform myRect;
    
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);

        myRect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        myRect.anchoredPosition += new Vector2(0f, -moveSpeed * Time.deltaTime);
    }
}
