using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
    public class SceneTransitionManager : MonoBehaviour
    {
        public SceneOverlay sceneOverlay;

        private string _currentSceneName;
        private bool _currentlyInLevel;
        private int _currentLevel;

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
            SetStartScene("StartScene");
        }

        public static SceneTransitionManager Get()
        {
            return _instance;
        }

        public bool IsInTransition()
        {
            return _inTransition;
        }

        private void SetStartScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            _currentSceneName = sceneName;
        }

        public void TransitionToNonLevelScene(string sceneName)
        {
            if (!_inTransition)
            {
                _inTransition = true;
                StartCoroutine(FadeInStartAndFadeOutScene(sceneName, false, 0));
            }
        }

        public void TransitionToLevel(int level)
        {
            if (!_inTransition)
            {
                _inTransition = true;
                StartCoroutine(FadeInStartAndFadeOutScene(GetLevelSceneName(level), true, level));
            }
        }

        private IEnumerator FadeInStartAndFadeOutScene(string levelName, bool isLevel, int level)
        {
            // Start Animation once faded in, Scene can be changed
            yield return sceneOverlay.CoverScreen().WaitForCompletion();
            
            if (_currentSceneName != null)
            {
                yield return SceneManager.UnloadSceneAsync(_currentSceneName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            }
            
            yield return SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
            
            _currentSceneName = levelName;
            _currentlyInLevel = isLevel;
            _currentLevel = level;
            _inTransition = false;

            // Scene has transitioned, now reverse Animation
            yield return sceneOverlay.UncoverScreen().WaitForCompletion();
        }

        public void TransitionToNextLevel()
        {
            if (null == _currentSceneName || !_currentlyInLevel)
            {
                Debug.LogError("Current Scene is not a level scene");
                return;
            }

            TransitionToLevel(_currentLevel + 1);
        }

        private string GetLevelSceneName(int level)
        {
            return "Level" + level + "Scene";
        }

        public void ReloadCurrentScene()
        {
            TransitionToNonLevelScene(_currentSceneName);
        }

        public int GetCurrentLevel()
        {
            return _currentLevel;
        }
    }
}