using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class CrossFade : SceneTransition
{
    public static CrossFade instance;
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
    
        //throw new NotImplementedException();
    }
    public CanvasGroup crossFade;
    public override IEnumerator AnimateTransitionIn()
    {
        var tweener = crossFade.DOFade(1f, 1f);
        yield return tweener.WaitForCompletion();
    }
    public override IEnumerator AnimateTransitionOut()
    {
        var tweener = crossFade.DOFade(0f, 1f);
        yield return tweener.WaitForCompletion();
    }
   
}
