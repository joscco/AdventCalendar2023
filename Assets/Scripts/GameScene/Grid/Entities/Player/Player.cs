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
        private float itemStackBaseVerticalOffset = 120;
        private float initialVerticalBodyOffset = -20;
        private float jumpHeight = 40;
        
        [SerializeField] private List<InteractableItem> pickedItems;

        [SerializeField] protected Transform flippablePart;
        [SerializeField] protected Transform offsettablePart;

        [SerializeField] private SpriteRenderer bodySpriteRenderer;
        [SerializeField] private Sprite spriteWhenCarrying;
        [SerializeField] private Sprite spriteWhenNormal;

        private Tween _bodySpriteMoveTween;

        public void ShowCarrying()
        {
            bodySpriteRenderer.sprite = spriteWhenCarrying;
        }

        public void ShowIdle()
        {
            bodySpriteRenderer.sprite = spriteWhenNormal;
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

        public Vector3 GetRelativeTopPosition()
        {
            var y = itemStackBaseVerticalOffset + InteractableItem.ItemHeight * pickedItems.Count;
            return new Vector3(0, y, 0);
        }

        public void TopWithItem(InteractableItem item)
        {
            var newRelativeTop = GetRelativeTopPosition();
            pickedItems.Add(item);
            ShowCarrying();

            item.AttachToPlayer(offsettablePart, offsettablePart.localPosition + newRelativeTop);
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

        public override void StopMoving()
        {
            base.StopMoving();
            _bodySpriteMoveTween?.Kill();
        }

        private Tween TweenGlobalMovePosition(Vector3 newGlobalPosition, float verticalOffset)
        {
            StopMoving();
            var currentPosition = transform.position;
            var distance = (currentPosition - new Vector3(newGlobalPosition.x, newGlobalPosition.y, currentPosition.z))
                .magnitude;
            var duration = 0.1f + distance * 0.0005f;
            var currentOffset = offsettablePart.transform.localPosition.y;
            var endOffset = initialVerticalBodyOffset + verticalOffset;
            _moveTween = transform
                .DOLocalMove(newGlobalPosition, duration)
                .SetEase(Ease.OutSine);
            _bodySpriteMoveTween = DOTween.Sequence()
                .Append(offsettablePart.transform
                    .DOLocalMoveY(Math.Max(currentOffset, endOffset) + jumpHeight, duration/2)
                    .SetEase(Ease.OutSine))
                .Append(offsettablePart.transform
                    .DOLocalMoveY(endOffset, duration/2)
                    .SetEase(Ease.InOutSine));
            return _moveTween;
        }
    }
}