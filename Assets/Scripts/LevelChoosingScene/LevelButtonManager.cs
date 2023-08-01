using System.Collections.Generic;
using DG.Tweening;
using GameScene.Options;
using SceneManagement;
using UI;
using UnityEngine;

namespace LevelChoosingScene
{
    public class LevelButtonManager : MonoBehaviour
    {
        [SerializeField] private List<LevelButton> buttons;
        [SerializeField] private SpriteRenderer selection;
        [SerializeField] private int unlockedLevelsForTesting;

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

            InitLevelButtons(_unlockedLevel);
            ChangeButtonFocus(_unlockedLevel, true);
        }

        private void InitLevelButtons(int highestLevelActive)
        {
            foreach (var button in buttons)
            {
                _buttonDict.Add(button.GetLevel(), button);

                // Set Events
                button.OnButtonHover += () =>
                {
                    var buttonLevel = button.GetLevel();
                    if (buttonLevel <= _unlockedLevel)
                    {
                        ChangeButtonFocus(button.GetLevel());
                    }
                };
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
                var newLevel = _focusedLevelButton.GetLevel() - 4;
                if (newLevel > 0)
                {
                    ChangeButtonFocus(newLevel);
                }

                return;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                var newLevel = _focusedLevelButton.GetLevel() + 4;
                if (newLevel <= _unlockedLevel)
                {
                    ChangeButtonFocus(newLevel);
                }
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                var newLevel = _focusedLevelButton.GetLevel() + 1;
                if (newLevel <= _unlockedLevel)
                {
                    ChangeButtonFocus(newLevel);
                }

                return;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                var newLevel = _focusedLevelButton.GetLevel() - 1;
                if (newLevel > 0)
                {
                    ChangeButtonFocus(newLevel);
                }
            }
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
            if (_focusedLevelButton)
            {
                _focusedLevelButton.ScaleDown();
            }

            var newButton = _buttonDict[newLevel];

            if (null != newButton)
            {
                newButton.ScaleUp();
                _focusedLevelButton = newButton;
                if (instant)
                {
                    selection.transform.position = newButton.transform.position;
                }
                else
                {
                    selection.transform.DOMove(newButton.transform.position, 0.2f)
                        .SetEase(Ease.OutBack);
                }
            }
        }
    }
}