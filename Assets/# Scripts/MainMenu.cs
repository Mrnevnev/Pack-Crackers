using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public string battleSelectScene;

    public string optionMenu, mainMenu, transition;
    
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlayMenuMusic();
        
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        AudioManager.instance.PlaySFX(0);
        SceneManager.LoadScene(battleSelectScene);
        
    }

    public void QuitGame()
    {
        Application.Quit();
        
        Debug.Log("Quitting Game");
        
        AudioManager.instance.PlaySFX(0);
    }

    public void Options()
    {
        SceneManager.LoadScene(optionMenu);
        
        
        AudioManager.instance.PlaySFX(0);
    }
    public void Main()
    {
        SceneManager.LoadScene(mainMenu);
    }
}