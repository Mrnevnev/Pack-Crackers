using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableButtons : MonoBehaviour
{
    public GameObject objectToDisable;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WhenButtonClick()
    {
        if (objectToDisable.activeInHierarchy == true)
        {
            objectToDisable.SetActive(false);
        }
        else
        {
            objectToDisable.SetActive(true);
        }
    }
}
