using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
    public class SceneTransitionManager : MonoBehaviour
    {
        public SceneOverlay sceneOverlay;
        
        private SceneReference _currentScene;
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
        
        private void Start()
        {
            SetStartScene(SceneReference.START);
        }

        public static SceneTransitionManager Get()
        {
            return _instance;
        }

        public bool IsInTransition()
        {
            return _inTransition;
        }
        
        private void SetStartScene(SceneReference scene)
        {
            SceneManager.LoadScene(scene.GetName(), LoadSceneMode.Additive);
            _currentScene = scene;
        }

        public void TransitionToScene(SceneReference scene)
        {
            if (!_inTransition)
            {
                StartCoroutine(FadeInStartAndFadeOutScene(scene));
            }
        }

        private IEnumerator FadeInStartAndFadeOutScene(SceneReference scene)
        {
            _inTransition = true;

            // Start Loading Scene and Level in Background
            AsyncOperation asyncSceneLoad = SceneManager.LoadSceneAsync(scene.GetName(), LoadSceneMode.Additive);
            asyncSceneLoad.allowSceneActivation = false;

            // Start Animation once faded in, Scene can be changed
            yield return sceneOverlay.CoverScreen().WaitForCompletion();
        
            if (_currentScene != null)
            {
                SceneManager.UnloadSceneAsync(_currentScene.GetName());
            }

            asyncSceneLoad.allowSceneActivation = true;
            _currentScene = scene;

            while (!asyncSceneLoad.isDone)
            {
                yield return null;
            }

            // Scene has transitioned, now reverse Animation
            yield return sceneOverlay.UncoverScreen().WaitForCompletion();
            _inTransition = false;
        }

        public void TransitionToNextLevel()
        {
            if (null == _currentScene || !_currentScene.IsLevel())
            {
                Debug.LogError("Current Scene is not a level scene");
                return;
            }
            
            TransitionToScene(_currentScene.GetNextLevel());
        }
    }
}