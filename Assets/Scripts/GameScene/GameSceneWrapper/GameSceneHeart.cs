

using SceneManagement;
using UI;
using UnityEngine;

namespace GameScene.GameSceneWrapper
{
    public class GameSceneHeart : MonoBehaviour
    {
        [SerializeField] private ScalingButton optionScreenButton;
        [SerializeField] private ScalingButton backToLevelsButton;
        [SerializeField] private ScalingButton retryButton;
        [SerializeField] private WinScreen.WinScreen winScreen;
        
        public static GameSceneHeart instance;

        private const KeyCode OptionScreenKey = KeyCode.P;
        private const KeyCode BackToLevelsKey = KeyCode.Q;
        private const KeyCode RetryKey = KeyCode.R;
        private LevelManager _levelManager;
        private GameSceneState _state = GameSceneState.Unpaused;

        private void Start()
        {
            instance = this;
            _levelManager = FindObjectOfType<LevelManager>();

            optionScreenButton.OnButtonHover += () => optionScreenButton.ScaleUp();
            optionScreenButton.OnButtonExit += () => optionScreenButton.ScaleDown();
            optionScreenButton.OnButtonClick += ActivateOptionsButton;
            backToLevelsButton.OnButtonHover += () => backToLevelsButton.ScaleUp();
            backToLevelsButton.OnButtonExit += () => backToLevelsButton.ScaleDown();
            backToLevelsButton.OnButtonClick += ActivateBackToLevelsButton;
            retryButton.OnButtonHover += () => retryButton.ScaleUp();
            retryButton.OnButtonExit += () => retryButton.ScaleDown();
            retryButton.OnButtonClick += ActivateRetryButton;
        }

        private void ActivateBackToLevelsButton()
        {
            SceneTransitionManager.Get().TransitionToNonLevelScene("LevelChoosingScene");
            backToLevelsButton.ScaleUpThenDown();
        }

        private void ActivateOptionsButton()
        {
            ToggleOptionScreen();
            optionScreenButton.ScaleUpThenDown();
        }
        
        private void ActivateRetryButton()
        {
            SceneTransitionManager.Get().ReloadCurrentScene();
            retryButton.ScaleUpThenDown();
        }

        private void Update()
        {
            if (Input.GetKeyDown(OptionScreenKey))
            {
                ActivateOptionsButton();
                return;
            }

            if (Input.GetKeyDown(BackToLevelsKey))
            {
                ActivateBackToLevelsButton();
                return;
            }
            
            if (Input.GetKeyDown(RetryKey))
            {
                ActivateRetryButton();
                return;
            }

            switch (_state)
            {
                case GameSceneState.Unpaused:
                    if (_levelManager.HasWon())
                    {
                        BlendInWinScreen();
                        break;
                    }
                    _levelManager.HandleUpdate();
                    break;
                case GameSceneState.ShowingWinScreen:
                    winScreen.HandleUpdate();
                    break;
                case GameSceneState.ShowingOptionScreen:
                    Options.OptionScreen.instance.HandleUpdate();
                    break;
            }
        }

        public void BlendInWinScreen()
        {
            winScreen.BlendIn(1f);
            _state = GameSceneState.ShowingWinScreen;
        }

        public void BlendInOptionScreen()
        {
            Options.OptionScreen.instance.BlendIn();
            _state = GameSceneState.ShowingOptionScreen;
        }

        public void BlendOutOptionScreen()
        {
            Options.OptionScreen.instance.BlendOut();
            _state = GameSceneState.Unpaused;
        }

        private void ToggleOptionScreen()
        {
            if (Options.OptionScreen.instance.IsShowing())
            {
                BlendOutOptionScreen();
            }
            else
            {
                BlendInOptionScreen();
            }
        }
    }

    internal enum GameSceneState
    {
        Unpaused,
        ShowingOptionScreen,
        ShowingWinScreen
    }
}