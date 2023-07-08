using DG.Tweening;
using GameScene.Grid.Entities.Shared;
using UnityEngine;

namespace GameScene.PlayerControl
{
    public class MovableGridEntity : GridEntity
    {
        [SerializeField] private bool flipInMovingDirection;
        [SerializeField] private Transform flippable;
        
        private bool _portaling;
        private Tween _moveTween;

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

        public Tween MoveTo(Vector2Int newIndex, Vector3 newPos)
        {
            var oldIndex = currentMainIndex;
            currentMainIndex = newIndex;

            if (flippable && flipInMovingDirection)
            {
                SwapFaceDirectionIfNecessary(oldIndex.x, newIndex.x);
            }

            return TweenGlobalMovePosition(newPos);
        }

        private void SwapFaceDirectionIfNecessary(int oldX, int newX)
        {
            bool shouldFaceLeft = newX < oldX;
            bool shouldFaceRight = newX > oldX;

            if (shouldFaceLeft)
            {
                flippable.transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (shouldFaceRight)
            {
                flippable.transform.localScale = new Vector3(1, 1, 1);
            }
        }

        private Tween TweenGlobalMovePosition(Vector3 newGlobalPosition)
        {
            StopMoving();
            var distance = (transform.position - new Vector3(newGlobalPosition.x, newGlobalPosition.y, transform.position.z)).magnitude;
            _moveTween = transform.DOMove(newGlobalPosition, 0.1f + distance * 0.0005f)
                .SetEase(Ease.OutSine);
            return _moveTween;
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