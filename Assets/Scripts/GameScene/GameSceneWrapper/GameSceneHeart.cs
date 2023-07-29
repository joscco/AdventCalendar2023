using Code.GameScene.UI;
using General.OptionScreen;
using SceneManagement;
using UnityEngine;

namespace GameScene.GameSceneWrapper
{
    public class GameSceneHeart : MonoBehaviour
    {
        [SerializeField] private ScalingButton optionScreenButton;
        [SerializeField] private ScalingButton backToLevelsButton;
        [SerializeField] private ScalingButton retryButton;
        [SerializeField] private WinScreen.WinScreen winScreen;
        
        private static GameSceneHeart _instance;

        private const KeyCode OptionScreenKey = KeyCode.P;
        private const KeyCode BackToLevelsKey = KeyCode.Q;
        private const KeyCode RetryKey = KeyCode.R;
        private LevelManager _levelManager;
        private GameSceneState _state = GameSceneState.Unpaused;

        public static GameSceneHeart Get()
        {
            return _instance;
        }

        private void Start()
        {
            _instance = this;
            _levelManager = FindObjectOfType<LevelManager>();

            optionScreenButton.OnButtonHover += () => optionScreenButton.Select();
            optionScreenButton.OnButtonExit += () => optionScreenButton.Deselect();
            optionScreenButton.OnButtonClick += ActivateOptionsButton;
            backToLevelsButton.OnButtonHover += () => backToLevelsButton.Select();
            backToLevelsButton.OnButtonExit += () => backToLevelsButton.Deselect();
            backToLevelsButton.OnButtonClick += ActivateBackToLevelsButton;
            retryButton.OnButtonHover += () => retryButton.Select();
            retryButton.OnButtonExit += () => retryButton.Deselect();
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
                    OptionScreen.instance.HandleUpdate();
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
            OptionScreen.instance.BlendIn();
            _state = GameSceneState.ShowingOptionScreen;
        }

        public void BlendOutOptionScreen()
        {
            OptionScreen.instance.BlendOut();
            _state = GameSceneState.Unpaused;
        }

        private void ToggleOptionScreen()
        {
            if (OptionScreen.instance.IsShowing())
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