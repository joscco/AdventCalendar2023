using DG.Tweening;
using UnityEngine;

namespace GameScene.Grid.Entities.Shared
{
    public abstract class MovableGridEntity : GridEntity
    {
        private bool _portaling;
        protected Tween _moveTween;

        protected override void OnDestroy()
        {
            _moveTween?.Kill();
        }

        public void InstantUpdatePosition(Vector2Int newIndex, Vector3 newPos)
        {
            StopMoving();
            currentMainIndex = newIndex;
            transform.position = newPos;
        }

        public virtual void StopMoving()
        {
            _moveTween?.Kill();
        }

        public bool IsPortaling()
        {
            return _portaling;
        }

        public void SetPortaling(bool value)
        {
            _portaling = value;
        }
    }
}