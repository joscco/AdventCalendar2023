

using SceneManagement;
using UI;
using UnityEngine;

namespace GameScene.GameSceneWrapper
{
    public class LevelSceneManager : MonoBehaviour
    {
        [SerializeField] private ScalingButton optionScreenButton;
        [SerializeField] private ScalingButton backToLevelsButton;
        [SerializeField] private ScalingButton retryButton;
        [SerializeField] private WinScreen.WinScreen winScreen;
        [SerializeField] private LevelManager _levelManager;

        private const KeyCode OptionScreenKey = KeyCode.P;
        private const KeyCode BackToLevelsKey = KeyCode.Q;
        private const KeyCode RetryKey = KeyCode.R;
        private LevelSceneState _state = LevelSceneState.Unpaused;

        private void Start()
        {
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
                case LevelSceneState.Unpaused:
                    if (_levelManager.HasWon())
                    {
                        BlendInWinScreen();
                        break;
                    }
                    _levelManager.HandleUpdate();
                    break;
                case LevelSceneState.ShowingWinScreen:
                    winScreen.HandleUpdate();
                    break;
                case LevelSceneState.ShowingOptionScreen:
                    Options.OptionScreen.instance.HandleUpdate();
                    break;
            }
        }

        public void BlendInWinScreen()
        {
            winScreen.BlendIn(1f);
            _state = LevelSceneState.ShowingWinScreen;
        }

        private void ActivateOptionsButton()
        {
            ToggleOptionScreen();
            optionScreenButton.ScaleUpThenDown();
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
        
        private void BlendInOptionScreen()
        {
            Options.OptionScreen.instance.BlendIn();
            _state = LevelSceneState.ShowingOptionScreen;
        }

        private void BlendOutOptionScreen()
        {
            Options.OptionScreen.instance.BlendOut();
            _state = LevelSceneState.Unpaused;
        }
    }

    internal enum LevelSceneState
    {
        Unpaused,
        ShowingOptionScreen,
        ShowingWinScreen
    }
}