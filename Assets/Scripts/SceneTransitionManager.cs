using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code
{
    public class SceneTransitionManager : MonoBehaviour
    {
        public CanvasGroup overlay;
        
        private float _transitionTimeInSeconds = 1f;
        private string _currentSceneName;
        private bool _inTransition;

        private static SceneTransitionManager _instance;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this);
            }
            else
            {
                _instance = this;
            }
        }

        public static SceneTransitionManager Get()
        {
            return _instance;
        }

        public bool IsInTransition()
        {
            return _inTransition;
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
            _inTransition = true;
            
            // Start Loading in Background
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
            asyncLoad.allowSceneActivation = false;

            // Start Animation
            overlay.DOFade(1, _transitionTimeInSeconds).SetEase(Ease.InOutQuad);

            // Once faded in, Scene can be changed
            yield return new WaitForSeconds(_transitionTimeInSeconds);
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
            overlay.DOFade(0, _transitionTimeInSeconds).SetEase(Ease.InOutQuad);
            
            _inTransition = false;
        }
    }
}