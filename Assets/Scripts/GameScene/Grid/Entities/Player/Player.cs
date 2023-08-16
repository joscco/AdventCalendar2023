using System;
using System.Collections.Generic;
using DG.Tweening;
using GameScene.Grid.Entities.ItemInteraction;
using GameScene.Grid.Entities.Shared;
using UnityEngine;

namespace GameScene.Grid.Entities.Player
{
    public class Player : MovableGridEntity
    {
        private float itemStackBaseVerticalOffset = 140;
        private float initialVerticalBodyOffset = 30;
        private float jumpHeight = 40;

        [SerializeField] private List<InteractableItem> pickedItems;

        [SerializeField] protected Transform flippablePart;
        [SerializeField] protected Transform offsettablePart;

        [SerializeField] private Transform leftArm;
        [SerializeField] private Transform rightArm;
        [SerializeField] private Transform leftLeg;
        [SerializeField] private Transform rightLeg;

        private Tween _bodySpriteMoveTween;
        private Tween _armRotateTween;
        private Tween _legRotateTween;

        public void ShowCarrying()
        {
            _armRotateTween?.Kill();
            _armRotateTween = DOTween.Sequence()
                .Append(leftArm.DOLocalRotate(new Vector3(0, 0, -60), 0.2f)
                    .SetEase(Ease.InOutQuad))
                .Join(rightArm.DOLocalRotate(new Vector3(0, 0, 60), 0.2f)
                    .SetEase(Ease.InOutQuad));
        }

        public void ShowIdle()
        {
            _armRotateTween?.Kill();
            _armRotateTween = DOTween.Sequence()
                .Append(leftArm.DOLocalRotate(new Vector3(0, 0, 60), 0.2f)
                    .SetEase(Ease.InOutQuad))
                .Join(rightArm.DOLocalRotate(new Vector3(0, 0, -60), 0.2f)
                    .SetEase(Ease.InOutQuad));
        }

        private Tween ShowJump()
        {
            _legRotateTween?.Kill();
            _legRotateTween = DOTween.Sequence()
                .Append(leftLeg.DOLocalRotate(new Vector3(0, 0, -40), 0.1f)
                    .SetEase(Ease.OutQuad))
                .Join(rightLeg.DOLocalRotate(new Vector3(0, 0, 40), 0.1f)
                    .SetEase(Ease.OutQuad));
            return _legRotateTween;
        }

        private Tween ShowNonJump()
        {
            _legRotateTween?.Kill();
            _legRotateTween = DOTween.Sequence()
                .Append(leftLeg.DOLocalRotate(new Vector3(0, 0, 0), 0.15f)
                    .SetEase(Ease.OutQuad))
                .Join(rightLeg.DOLocalRotate(new Vector3(0, 0, 0), 0.15f)
                    .SetEase(Ease.OutQuad));
            return _legRotateTween;
        }

        protected void Jump()
        {
            DOTween.Sequence()
                .Append(ShowJump())
                .Append(ShowNonJump());
        }

        public bool IsBearingItem()
        {
            return pickedItems.Count > 0;
        }

        public InteractableItem GetTopItem()
        {
            if (IsBearingItem())
            {
                return pickedItems[^1];
            }

            return null;
        }

        public float GetRelativeTop()
        {
            return itemStackBaseVerticalOffset + InteractableItem.ItemHeight * pickedItems.Count;
        }

        public void TopWithItem(InteractableItem item)
        {
            pickedItems.Add(item);
            ShowCarrying();
        }

        public void RemoveItem(InteractableItem item)
        {
            pickedItems.Remove(item);
            if (!IsBearingItem())
            {
                ShowIdle();
            }
        }

        public void PlayDeathAnimation()
        {
            Debug.Log("Player Died!");
        }

        public void PlayWinAnimation()
        {
            Debug.Log("Player Won!");
        }

        public Tween MoveTo(Vector2Int newIndex, Vector3 newPos, float verticalOffset)
        {
            currentMainIndex = newIndex;
            return TweenGlobalMovePosition(newPos, verticalOffset);
        }

        public void SwapFaceDirectionIfNecessary(int newX)
        {
            var oldX = currentMainIndex.x;
            bool shouldFaceLeft = newX < oldX;
            bool shouldFaceRight = newX > oldX;

            if (shouldFaceLeft)
            {
                flippablePart.transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (shouldFaceRight)
            {
                flippablePart.transform.localScale = new Vector3(1, 1, 1);
            }
        }

        public void StopMoving()
        {
            _bodySpriteMoveTween?.Kill();
        }

        private Tween TweenGlobalMovePosition(Vector3 newGlobalPosition, float verticalOffset)
        {
            StopMoving();
            Jump();
            Vector2 dir = (transform.position - newGlobalPosition);
            var duration = 0.2f + dir.magnitude * 0.00001f;

            _moveTween = transform
                .DOLocalMove(newGlobalPosition, duration)
                .SetEase(Ease.OutSine);

            // Jump Animation:
            var currentVerticalOffset = offsettablePart.transform.localPosition.y;
            var endVerticalOffset = initialVerticalBodyOffset + verticalOffset;

            _bodySpriteMoveTween = DOTween.Sequence()
                .Append(offsettablePart.transform
                    .DOLocalMoveY(Math.Max(currentVerticalOffset, endVerticalOffset) + jumpHeight, duration / 2)
                    .SetEase(Ease.OutSine))
                .Append(offsettablePart.transform
                    .DOLocalMoveY(endVerticalOffset, duration / 2)
                    .SetEase(Ease.InOutSine));

            return _bodySpriteMoveTween;
        }

        public Transform GetOffsettablePart()
        {
            return offsettablePart.transform;
        }
    }
}