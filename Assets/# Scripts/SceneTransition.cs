using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneTransition : MonoBehaviour
{
    public abstract IEnumerator AnimateTransitionIn();
    public abstract IEnumerator AnimateTransitionOut();
}
// public class SomeSceneTransition : SceneTransition 
// {
//     public override IEnumerator AnimateTransitionIn()
//     {
//         // <your_code_here>
//         yield return null; // replace with your actual implementation
//     }
//
//     public override IEnumerator AnimateTransitionOut()
//     {
//         // <your_code_here>
//         yield return null; // replace with your actual implementation
//     }
// }
