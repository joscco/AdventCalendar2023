using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene.Options
{
    public class LeftRightOptionGroup : MonoBehaviour
    {
        public Action wantsFocus;
        public Action<int> changeValue;
        
        [SerializeField] private List<OnOffButton> optionsFromLeftToRight;
        [SerializeField] private int currentIndex;
        [SerializeField] private bool incrementalValues;
        private int _maxValue;

        private void Start()
        {
            _maxValue = optionsFromLeftToRight.Count - 1;
            for (int index = 0; index <= _maxValue; index++)
            {
                var curIndex = index;
                var button = optionsFromLeftToRight[curIndex];

                button.OnButtonHover += () => FocusButton(button);
                button.OnButtonExit += () => DefocusButton(button);
                button.OnButtonClick += () => SetValue(curIndex);
            }
            
            SetValue(currentIndex);
        }

        public void SetValue(int value)
        {
            currentIndex = value;
            changeValue?.Invoke(value);

            if (incrementalValues)
            {
                for (int i = 0; i < optionsFromLeftToRight.Count; i++)
                {
                    if (i <= value)
                    {
                        optionsFromLeftToRight[i].SetOn();
                    }
                    else
                    {
                        optionsFromLeftToRight[i].SetOff();
                    }
                }
            }
            else
            {
                for (int i = 0; i < optionsFromLeftToRight.Count; i++)
                {
                    if (i == value)
                    {
                        optionsFromLeftToRight[i].SetOn();
                    }
                    else
                    {
                        optionsFromLeftToRight[i].SetOff();
                    }
                }
            }

            optionsFromLeftToRight[value].ScaleUpThenDown();
        }

        public void IncreaseValue()
        {
            SetValue(Math.Min(GetValue() + 1, _maxValue));
        }

        public void DecreaseValue()
        {
            SetValue(Math.Max(GetValue() - 1, 0));
        }

        public int GetValue()
        {
            return currentIndex;
        }

        private void DefocusButton(OnOffButton button)
        {
            button.ScaleDown();
        }

        private void FocusButton(OnOffButton button)
        {
            wantsFocus.Invoke();
            button.ScaleUp();
        }
    }
}