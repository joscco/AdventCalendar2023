using System;
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code
{
    public class SceneTransitionManager : MonoBehaviour
    {
        public float transitionTimeInSeconds = 1f;
        public CanvasGroup overlay;
        private string _currentSceneName;

        private static SceneTransitionManager _instance;

        private void Awake()
        {
            // If there is an instance, and it's not me, delete myself.

            if (_instance != null && _instance != this)
            {
                Destroy(this);
            }
            else
            {
                _instance = this;
            }
        }

        public static SceneTransitionManager get()
        {
            return _instance;
        }

        private void Start()
        {
            overlay.alpha = 0;
            TransitionTo("StartScene");
        }

        public void TransitionTo(String levelName)
        {
            StartCoroutine(FadeInStartAndFadeOut(levelName));
        }

        private IEnumerator FadeInStartAndFadeOut(String levelName)
        {
            // Start Loading in Background
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
            asyncLoad.allowSceneActivation = false;

            // Start Animation
            overlay.DOFade(1, transitionTimeInSeconds).SetEase(Ease.InOutQuad);

            // Once faded in, Scene can be changed
            yield return new WaitForSeconds(transitionTimeInSeconds);
            if (_currentSceneName != null)
            {
                SceneManager.UnloadSceneAsync(_currentSceneName);
            }

            asyncLoad.allowSceneActivation = true;
            _currentSceneName = levelName;

            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            // Scene has transitioned, now reverse Animation
            overlay.DOFade(0, transitionTimeInSeconds).SetEase(Ease.InOutQuad);
        }
    }
}