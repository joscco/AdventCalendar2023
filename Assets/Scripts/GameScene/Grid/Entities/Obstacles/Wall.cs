using DG.Tweening;
using GameScene.Grid.Entities.Shared;
using UnityEngine;

namespace GameScene.Grid.Entities.Obstacles
{
    public class Wall : GridEntity
    {
        [SerializeField] private bool blocking = true;

        public void Unblock()
        {
            blocking = false;
            Hide();
        }

        public bool IsBlocking()
        {
            return blocking;
        }

        private void Hide()
        {
            transform.DOScale(0, 0.3f).SetEase(Ease.InBack);
        }
    }
}