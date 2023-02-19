using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public float transitionTimeInSeconds = 1f;
    public Animator transitionAnimator;
    private static readonly int FadeIn = Animator.StringToHash("FadeIn");
    private static readonly int FadeOut = Animator.StringToHash("FadeOut");

    public void LoadLevel(String levelName)
    {
        StartCoroutine(PlayAndTransition(levelName));
    }

    private IEnumerator PlayAndTransition(String levelName)
    {
        // Start Loading in Background
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName);
        asyncLoad.allowSceneActivation = false;
        
        // Start Animation
        transitionAnimator.SetTrigger(FadeIn);
        
        // Once done, Scene can be changed
        yield return new WaitForSeconds(transitionTimeInSeconds);
        asyncLoad.allowSceneActivation = true;
        
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
        // Scene has transitioned, now reverse Animation
        transitionAnimator.SetTrigger(FadeOut);
    }
}
