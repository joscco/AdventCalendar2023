using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public float transitionTimeInSeconds = 1f;
    public CanvasGroup overlay;

    private void Start()
    {
        overlay.alpha = 0;
    }

    public void TransitionTo(String levelName)
    {
        StartCoroutine(FadeInStartAndFadeOut(levelName));
    }

    private IEnumerator FadeInStartAndFadeOut(String levelName)
    {
        // Start Loading in Background
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName);
        asyncLoad.allowSceneActivation = false;
        
        // Start Animation
        overlay.DOFade(1, transitionTimeInSeconds).SetEase(Ease.InOutQuad);
        
        // Once faded in, Scene can be changed
        yield return new WaitForSeconds(transitionTimeInSeconds);
        asyncLoad.allowSceneActivation = true;
        
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
        // Scene has transitioned, now reverse Animation
        overlay.DOFade(1, transitionTimeInSeconds).SetEase(Ease.InOutQuad);
    }
}
