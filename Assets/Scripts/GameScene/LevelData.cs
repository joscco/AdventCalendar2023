using GameScene.UI;
using UnityEngine;

namespace GameScene
{
    public class LevelData : MonoBehaviour
    {
        [SerializeField] private int startActions = 5;
        [SerializeField] private ActionsLabel actionsLabel;
        private int _actionsLeft;

        private void Start()
        {
            _actionsLeft = startActions;
        
            actionsLabel.SetActionsLeft(_actionsLeft);
        }
        
        public void RemoveAction()
        {
            if (_actionsLeft >= 1)
            {
                RemoveActionAndUpdate();
            }
        }

        public bool HasActionsLeft()
        {
            return _actionsLeft > 0;
        }

        private void RemoveActionAndUpdate()
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
