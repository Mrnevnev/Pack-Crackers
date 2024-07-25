using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance;
    public Slider progressBar;
    
    
    public GameObject transitionContainer;

    private SceneTransition[] _transitions;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);        
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _transitions = transitionContainer.GetComponentsInChildren<SceneTransition>();
    }

    public void LoadScene(string sceneName, string transitionName)
    {
        StartCoroutine(LoadSceneAsync(sceneName, transitionName));
    }

    private IEnumerator LoadSceneAsync(string sceneName, string transitionName)
    {
        SceneTransition transition = _transitions.First(t => t.name == transitionName);

        AsyncOperation scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        yield return transition.AnimateTransitionIn();

        progressBar.gameObject.SetActive(true);

        do
        {
            progressBar.value = scene.progress;
            yield return null;
        } while (scene.progress < 0.9f);

        scene.allowSceneActivation = true;
        progressBar.gameObject.SetActive(false);

        yield return transition.AnimateTransitionOut();
    }
}
