using DG.Tweening;
using UnityEngine;

namespace GameScene.Grid.Entities.Shared
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

        public Tween MoveTo(Vector2Int newIndex, Vector3 newPos, bool jump = false)
        {
            SwapFaceDirectionIfNecessary(newIndex.x);
            currentMainIndex = newIndex;
            return jump ? TweenGlobalJumpPosition(newPos) : TweenGlobalMovePosition(newPos);
        }

        public Tween LocalMoveTo(Vector3 newPos)
        {
            currentMainIndex = Vector2Int.zero;
            return TweenLocalMovePosition(newPos);
        }

        public void SwapFaceDirectionIfNecessary(int newX)
        {
            if (flippable && flipInMovingDirection)
            {
                var oldX = currentMainIndex.x;
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
        }

        private Tween TweenGlobalMovePosition(Vector3 newGlobalPosition)
        {
            StopMoving();
            var currentPosition = transform.position;
            var distance = (currentPosition - new Vector3(newGlobalPosition.x, newGlobalPosition.y, currentPosition.z)).magnitude;
            var duration = 0.1f + distance * 0.0005f;
            _moveTween = transform.DOLocalMove(newGlobalPosition, duration).SetEase(Ease.OutSine);
            return _moveTween;
        }

        private Tween TweenGlobalJumpPosition(Vector3 newGlobalPosition)
        {
            StopMoving();
            var currentPosition = transform.position;
            var distance = (currentPosition - new Vector3(newGlobalPosition.x, newGlobalPosition.y, currentPosition.z)).magnitude;
            var duration = 0.1f + distance * 0.0005f;
            var middlePosition = (currentPosition + newGlobalPosition) / 2;
            var offSetMiddlePosition = middlePosition + 30 * Vector3.up;
            _moveTween = DOTween.Sequence()
                .Append(transform.DOLocalMove(offSetMiddlePosition, duration / 2).SetEase(Ease.OutSine))
                .Append(transform.DOLocalMove(newGlobalPosition, duration / 2).SetEase(Ease.OutSine));
            return _moveTween;
        }

        private Tween TweenLocalMovePosition(Vector3 newLocalPosition)
        {
            StopMoving();
            var currentLocalPosition = transform.localPosition;
            var middlePosition = (currentLocalPosition + newLocalPosition) / 2;
            var offSetMiddlePosition = middlePosition + 75 * Vector3.up;
            _moveTween = DOTween.Sequence()
                .Append(transform.DOLocalMove(offSetMiddlePosition, 0.075f).SetEase(Ease.OutSine))
                .Append(transform.DOLocalMove(newLocalPosition, 0.075f).SetEase(Ease.OutSine));
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