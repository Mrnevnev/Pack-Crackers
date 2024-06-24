using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    
    
    public TextMeshProUGUI nameTextUI;
    public TextMeshProUGUI dialogueTextUI;
    public Image characterImage;
    public string sceneToLoad;
    public Animator animator;
    public SceneTransition transition;
    
    private Queue<string> sentences;
    
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("Is Open", true);
        
        nameTextUI.text = dialogue.name;
            
         characterImage.sprite = dialogue.characterSprite;
        
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueTextUI.text = sentence;


    }

    void EndDialogue()
    {
        animator.SetBool("Is Open", false);
        // Insert code here to perform any necessary actions before loading the scene
        SceneManager.LoadScene(sceneToLoad);
    }

}
