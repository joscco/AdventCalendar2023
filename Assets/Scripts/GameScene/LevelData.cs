using System;
using System.Linq;
using GameScene.Items.Field;
using GameScene.Items.Item;
using GameScene.UI;
using UnityEngine;

namespace GameScene
{
    public class LevelData : MonoBehaviour
    {
        [SerializeField] private int startActions = 5;
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
            _percentageNeeded = 0.8f;
            _percentageCalculator = grid =>
            {
                var fieldSpots = grid.GetFieldSpots();
                return fieldSpots
                    .Where(spot => spot.GetPlantData())
                    .Select(data => data.GetPlantData().itemType)
                    .Count(type => type == ItemType.Roses) * 1f / fieldSpots.Length ;
            };

            progressBar.SetStar(0.5f);
            progressBar.SetStar(0.65f);
            progressBar.SetStar(_percentageNeeded);
        }

        public void RecalculatePercentage(FieldGrid grid)
        {
            _percentageAchieved = _percentageCalculator.Invoke(grid);
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
