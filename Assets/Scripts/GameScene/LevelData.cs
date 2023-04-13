using System;
using GameScene.Items.Field;
using GameScene.UI;
using UnityEngine;

namespace GameScene
{
    public class LevelData : MonoBehaviour
    {
        [SerializeField] private int startActions = 10;
        [SerializeField] private ActionsLabel actionsLabel;
        [SerializeField] private ProgressBar progressBar;

        private Func<FieldGrid, float> _percentageCalculator;
        private int _actionsLeft;
        private float _percentageAchieved;
        private float _percentageNeeded;

        private void Start()
        {
            _actionsLeft = startActions;
            actionsLabel.SetActionsLeft(_actionsLeft);
            progressBar.InstantSetPercentFinished(0);
            
            // Just for now
            _percentageNeeded = 0.9f;

            progressBar.SetStar(0.5f);
            progressBar.SetStar(0.75f);
            progressBar.SetStar(_percentageNeeded);
        }

        public void UpdatePercentage(float percentageAchieved)
        {
            _percentageAchieved = percentageAchieved;
            progressBar.SetPercentFinished(_percentageAchieved);

            if (_percentageAchieved >= _percentageNeeded)
            {
                Level.Get().winScreen.BlendIn();
            }
        }

        public bool HasActionsLeft()
        {
            return _actionsLeft > 0;
        }

        public void RemoveAction()
        {
            if (_actionsLeft >= 1)
            {
                if (_actionsLeft == 1)
                {
                    Level.Get().winScreen.BlendIn();
                }

                _actionsLeft--;
                actionsLabel.SetActionsLeft(_actionsLeft);
            }
        }
    }
}
