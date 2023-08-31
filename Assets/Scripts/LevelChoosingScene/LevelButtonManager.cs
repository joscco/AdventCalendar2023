using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameScene;
using GameScene.Options;
using SceneManagement;
using UI;
using UnityEngine;

namespace LevelChoosingScene
{
    public class LevelButtonManager : MonoBehaviour
    {
        private List<LevelButton> _buttons;
        [SerializeField] private ScalingButton leftButton;
        [SerializeField] private ScalingButton rightButton;
        [SerializeField] private int unlockedLevelsForTesting;
        [SerializeField] private Transform buttonContainer;

        private readonly Dictionary<int, LevelButton> _buttonDict = new();
        private int _unlockedLevel;
        private LevelButton _focusedLevelButton;

        private const KeyCode OptionScreenKey = KeyCode.P;
        private const KeyCode BackScreenKey = KeyCode.Q;
        private const KeyCode StartKey = KeyCode.Space;

        [SerializeField] private ScalingButton optionScreenButton;
        [SerializeField] public ScalingButton backToStartButton;

        private void Start()
        {
            _unlockedLevel = Game.instance
                ? Game.instance.GetUnlockedLevels()
                : unlockedLevelsForTesting;

            optionScreenButton.OnButtonHover += () => optionScreenButton.ScaleUp();
            optionScreenButton.OnButtonExit += () => optionScreenButton.ScaleDown();
            optionScreenButton.OnButtonClick += ActivateOptionsButton;
            
            backToStartButton.OnButtonHover += () => backToStartButton.ScaleUp();
            backToStartButton.OnButtonExit += () => backToStartButton.ScaleDown();
            backToStartButton.OnButtonClick += ActivateBackToStartSceneButton;

            leftButton.OnButtonHover += () => leftButton.ScaleUp();
            leftButton.OnButtonExit += () => leftButton.ScaleDown();
            leftButton.OnButtonClick += DecreaseSelectedLevel;
            
            rightButton.OnButtonClick += IncreaseSelectedLevel;
            rightButton.OnButtonHover += () => rightButton.ScaleUp();
            rightButton.OnButtonExit += () => rightButton.ScaleDown();

            InitLevelButtons(_unlockedLevel);
            ChangeButtonFocus(_unlockedLevel, true);
        }

        private void InitLevelButtons(int highestLevelActive)
        {
            _buttons = GetComponentsInChildren<LevelButton>().ToList();
            foreach (var button in _buttons)
            {
                _buttonDict.Add(button.GetLevel(), button);

                button.OnButtonClick += () => SceneTransitionManager.Get().TransitionToLevel(button.GetLevel());

                if (button.GetLevel() <= highestLevelActive)
                {
                    button.TurnOn();
                }
                else
                {
                    button.TurnOff();
                }
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(BackScreenKey))
            {
                ActivateBackToStartSceneButton();
                return;
            }

            if (Input.GetKeyDown(OptionScreenKey))
            {
                ActivateOptionsButton();
                return;
            }

            if (Input.GetKeyDown(StartKey) && null != _focusedLevelButton)
            {
                ActivateSelectedLevelButton();
            }

            // Updating Option Screen if necessary
            if (OptionScreen.instance && OptionScreen.instance.IsShowing())
            {
                OptionScreen.instance.HandleUpdate();
                return;
            }

            // Arrow KeyHandling when not in option screen
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                DecreaseSelectedLevel();
                return;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                IncreaseSelectedLevel();
            }
        }

        private void DecreaseSelectedLevel()
        {
            var newLevel = _focusedLevelButton.GetLevel() - 1;
            if (newLevel > 0)
            {
                ChangeButtonFocus(newLevel);
            }

            leftButton.ScaleUpThenDown();
        }

        private void IncreaseSelectedLevel()
        {
            var newLevel = _focusedLevelButton.GetLevel() + 1;
            if (newLevel <= _unlockedLevel)
            {
                ChangeButtonFocus(newLevel);
            }
            
            rightButton.ScaleUpThenDown();
        }

        private void ActivateBackToStartSceneButton()
        {
            SceneTransitionManager.Get().TransitionToNonLevelScene("StartScene");
            backToStartButton.ScaleUpThenDown();
        }

        private void ActivateSelectedLevelButton()
        {
            SceneTransitionManager.Get().TransitionToLevel(_focusedLevelButton.GetLevel());
            _focusedLevelButton.ScaleUpThenDown();
        }

        private void ActivateOptionsButton()
        {
            OptionScreen.instance.Toggle();
            optionScreenButton.ScaleUpThenDown();
        }

        private void ChangeButtonFocus(int newLevel, bool instant = false)
        {
            if (instant)
            {
                buttonContainer.localPosition = new Vector2(-(newLevel - 1) * LevelButton.Width, buttonContainer.localPosition.y);
            }
            else
            {
                buttonContainer.DOLocalMoveX(-(newLevel - 1) * LevelButton.Width, 0.3f)
                    .SetEase(Ease.InOutBack);
            }


            if (_focusedLevelButton)
            {
                _focusedLevelButton.ScaleDown();
            }

            var newButton = _buttonDict[newLevel];

            if (null != newButton)
            {
                newButton.ScaleUp();
                _focusedLevelButton = newButton;
            }
        }
    }
}