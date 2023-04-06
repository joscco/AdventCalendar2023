using System;
using GameScene;
using UnityEngine;

namespace Code.GameScene
{
    public class LevelData : MonoBehaviour
    {
        [SerializeField] private int startMoney = 5;
        [SerializeField] private int startDays = 1;
        [SerializeField] private int actionsPerDay = 3;

        [SerializeField] private MoneyLabel _moneyLabel;
        [SerializeField] private ActionsLabel _actionsLabel;

        private bool timeOver;
    
        private int _money;
        private int _daysLeft;
        private int _actionsLeftOnCurrentDay;

        private void Start()
        {
            timeOver = false;
            _money = startMoney;
            _daysLeft = startDays;
            _actionsLeftOnCurrentDay = actionsPerDay;
        
            _moneyLabel.SetMoney(_money);
            _actionsLabel.SetDaysLeft(_daysLeft);
            _actionsLabel.SetActionsLeft(_actionsLeftOnCurrentDay);
            _actionsLabel.SetActionsPerDay(actionsPerDay);
        }

        public void AddMoney(int amount)
        {
            if (amount < 0)
            {
                throw new Exception("Money amount must be >= 0");
            }

            _money += amount;
            _moneyLabel.SetMoney(_money);
        }
    
        public void SubtractMoney(int amount)
        {
            if (amount < 0)
            {
                throw new Exception("Money amount must be >= 0");
            }

            _money = Math.Max(0, _money - amount);
            _moneyLabel.SetMoney(_money);
        }
    
        public int GetMoney()
        {
            return _money;
        }

        public void StartNextDay()
        {
            if (_daysLeft > 0)
            {
                _daysLeft--;
                _actionsLeftOnCurrentDay = actionsPerDay;
            }
            else
            {
                Level.Get().winScreen.BlendIn();
                timeOver = true;
            }
        
            _actionsLabel.SetDaysLeft(_daysLeft);
            _actionsLabel.SetActionsLeft(_actionsLeftOnCurrentDay);
        }

        public void RemoveAction()
        {
            if (_daysLeft >= 0 && _actionsLeftOnCurrentDay > 0)
            {
                _actionsLeftOnCurrentDay--;
                if (_actionsLeftOnCurrentDay == 0)
                {
                    StartNextDay();
                }
            }
        
            _actionsLabel.SetDaysLeft(_daysLeft);
            _actionsLabel.SetActionsLeft(_actionsLeftOnCurrentDay);
        }
        
        public bool NewDayHasStarted()
        {
            return _actionsLeftOnCurrentDay == actionsPerDay;
        }

        public bool HasActionsLeft()
        {
            return _actionsLeftOnCurrentDay > 0;
        }
    }
}
