using GameScene;
using GameScene.UI;
using General.LoseScreen;
using General.OptionScreen;
using General.WinScreen;
using UnityEngine;

namespace SceneManagement
{
    public class GameSceneHeart : MonoBehaviour
    {
        private static GameSceneHeart _instance;

        private const KeyCode OPTION_SCREEN_KEY = KeyCode.O;
        private const KeyCode BACK_TO_LEVELS_KEY = KeyCode.Q;

        public static GameSceneHeart Get()
        {
            return _instance;
        }

        [SerializeField] private WinScreen winScreen;
        [SerializeField] private OptionScreen optionScreen;
        [SerializeField] private LoseScreen loseScreen;

        [SerializeField] private OptionButton optionButton;
        [SerializeField] private GameSceneBackButton backButton;

        private LevelManager _levelManager;
        private GameSceneState _state = GameSceneState.UNPAUSED;

        private void Start()
        {
            _instance = this;
            _levelManager = FindObjectOfType<LevelManager>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(OPTION_SCREEN_KEY))
            {
                optionButton.Activate();
                return;
            }

            if (Input.GetKeyDown(BACK_TO_LEVELS_KEY))
            {
                backButton.Activate();
                return;
            }

            switch (_state)
            {
                case GameSceneState.UNPAUSED:
                    if (_levelManager.HasWon())
                    {
                        BlendInWinScreen();
                        break;
                    }
                    
                    if (_levelManager.HasLost())
                    {
                        BlendInLoseScreen();;
                        break;
                    }
                    _levelManager.HandleUpdate();
                    break;
                case GameSceneState.SHOWING_WIN_SCREEN:
                    winScreen.HandleUpdate();
                    break;
                case GameSceneState.SHOWING_LOSE_SCREEN:
                    loseScreen.HandleUpdate();
                    break;
                case GameSceneState.SHOWING_OPTION_SCREEN:
                    optionScreen.HandleUpdate();
                    break;
            }
        }

        public void BlendInWinScreen()
        {
            winScreen.BlendIn();
            _state = GameSceneState.SHOWING_WIN_SCREEN;
        }

        public void BlendInLoseScreen()
        {
            loseScreen.BlendIn();
            _state = GameSceneState.SHOWING_LOSE_SCREEN;
        }

        public void BlendInOptionScreen()
        {
            optionScreen.BlendIn();
            _state = GameSceneState.SHOWING_OPTION_SCREEN;
        }

        public void BlendOutOptionScreen()
        {
            optionScreen.BlendOut();
            _state = GameSceneState.UNPAUSED;
        }

        public void ToggleOptionScreen()
        {
            if (optionScreen.IsShowing())
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
        UNPAUSED,
        SHOWING_OPTION_SCREEN,
        SHOWING_WIN_SCREEN,
        SHOWING_LOSE_SCREEN
    }
}