using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace LevelChoosingScene
{
    public class LevelButtonManager : MonoBehaviour
    {
        [SerializeField] private List<LevelButton> buttons;
        [SerializeField] private SpriteRenderer selection;
        [SerializeField] private int unlockedLevelsForTesting;
        [SerializeField] private int availableLevelsForTesting;

        private Dictionary<int, LevelButton> buttonDict = new();
        private int unlockedLevel;
        private int availableLevels;
        private LevelButton selectedButton;
        
        private const KeyCode OptionScreenKey = KeyCode.P;
        private const KeyCode BackScreenKey = KeyCode.Q;
        private const KeyCode StartKey = KeyCode.Space;

        public OptionButton optionButton;
        public LevelChoosingSceneBackButton backButton;

        private void Start()
        {
            unlockedLevel = Game.instance
                ? Game.instance.GetHighestUnlockedLevel()
                : unlockedLevelsForTesting;

            availableLevels = Game.instance
                ? Game.AVAILABLE_LEVELS
                : availableLevelsForTesting;

            InitLevelButtons(unlockedLevel);
            ChangeButtonSelection(unlockedLevel, true);
        }

        private void InitLevelButtons(int highestLevelActive)
        {
            foreach (var button in buttons)
            {
                buttonDict.Add(button.GetLevel(), button);

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
                backButton.Activate();
                return;
            }
            
            if (Input.GetKeyDown(OptionScreenKey))
            {
                optionButton.Activate();
                return;
            }

            if (Input.GetKeyDown(StartKey) && null != selectedButton)
            {
                selectedButton.Activate();
            }

            // Arrow KeyHandling
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                var newLevel = selectedButton.GetLevel() - 4;
                if (newLevel > 0)
                {
                    ChangeButtonSelection(newLevel);
                }

                return;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                var newLevel = selectedButton.GetLevel() + 4;
                if (newLevel <= unlockedLevel)
                {
                    ChangeButtonSelection(newLevel);
                }
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                var newLevel = selectedButton.GetLevel() + 1;
                if (newLevel <= unlockedLevel)
                {
                    ChangeButtonSelection(newLevel);
                }

                return;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                var newLevel = selectedButton.GetLevel() - 1;
                if (newLevel > 0)
                {
                    ChangeButtonSelection(newLevel);
                }

            }
        }

        private void ChangeButtonSelection(int newLevel, bool instant = false)
        {
            if (selectedButton)
            {
                selectedButton.Deselect();
            }

            var newButton = buttonDict[newLevel];

            if (null != newButton)
            {
                newButton.Select();
                selectedButton = newButton;
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