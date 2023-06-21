using System;
using DG.Tweening;
using General.Grid;
using Unity.VisualScripting;
using UnityEngine;

namespace GameScene.PlayerControl
{
    public class MovableGridEntity : GridEntity
    {
        [SerializeField] private bool flipInMovingDirection;
        private Tween _moveTween;

        protected void OnDestroy()
        {
            base.OnDestroy();
            _moveTween?.Kill();
        }

        public void InstantUpdatePosition(Vector2Int newIndex, Vector2 newPos)
        {
            StopMoving();
            transform.position = newPos;
        }

        public void StopMoving()
        {
            _moveTween?.Kill();
        }

        public Tween MoveTo(Vector2Int newIndex, Vector2 newPos, MoveVariant moveVariant = MoveVariant.Global)
        {
            var oldIndex = currentMainIndex;
            currentMainIndex = newIndex;
            spriteRenderers.ForEach(rend => rend.sortingOrder = -newIndex.y);

            if (flipInMovingDirection)
            {
                SwapFaceDirectionIfNecessary(oldIndex.x, newIndex.x);
            }

            return moveVariant == MoveVariant.Global
                ? TweenGlobalMovePosition(newPos)
                : TweenLocalMovePosition(newPos);
        }

        private void SwapFaceDirectionIfNecessary(int oldX, int newX)
        {
            bool shouldFaceLeft = newX < oldX;
            bool shouldFaceRight = newX > oldX;

            if (shouldFaceLeft)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (shouldFaceRight)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }

        private Tween TweenGlobalMovePosition(Vector2 newGlobalPosition)
        {
            StopMoving();
            var distance = (transform.position - new Vector3(newGlobalPosition.x, newGlobalPosition.y, 0)).magnitude;
            _moveTween = transform.DOMove(newGlobalPosition, 0.1f + distance * 0.0005f)
                .SetEase(Ease.OutSine);
            return _moveTween;
        }

        private Tween TweenLocalMovePosition(Vector2 newLocalPosition)
        {
            StopMoving();
            var distance = (transform.localPosition - new Vector3(newLocalPosition.x, newLocalPosition.y, 0)).magnitude;
            _moveTween = transform.DOLocalMove(newLocalPosition, 0.1f + distance * 0.0005f)
                .SetEase(Ease.OutSine);
            return _moveTween;
        }

        public enum MoveVariant
        {
            Global,
            Local
        }
    }
}