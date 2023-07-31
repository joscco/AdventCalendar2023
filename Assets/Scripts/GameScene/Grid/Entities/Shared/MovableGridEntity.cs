using DG.Tweening;
using UnityEngine;

namespace GameScene.Grid.Entities.Shared
{
    public abstract class MovableGridEntity : GridEntity
    {
        [SerializeField] protected bool flipInMovingDirection;
        [SerializeField] protected Transform flippable;

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

        public void StopMoving()
        {
            _moveTween?.Kill();
        }

        public abstract Tween MoveTo(Vector2Int newIndex, Vector3 newPos);

        public abstract Tween JumpTo(Vector2Int newIndex, Vector3 newPos);

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