using DG.Tweening;
using GameScene.Grid.Entities.ItemInteraction;
using GameScene.Grid.Entities.Shared;
using UnityEngine;

namespace GameScene.Grid.Entities.Player
{
    public class Player : MovableGridEntity, IInteractableItemBearer
    {
        [SerializeField] private Transform itemPositionWhenPickedUp;
        [SerializeField] private InteractableItem pickedItem;

        [SerializeField] private SpriteRenderer bodySpriteRenderer;
        [SerializeField] private Sprite spriteWhenCarrying;
        [SerializeField] private Sprite spriteWhenNormal;
        
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
            return pickedItem != null;
        }

        public bool CanBeToppedWithItem(InteractableItem item)
        {
            return !IsBearingItem();
        }

        public InteractableItem GetItem()
        {
            return pickedItem;
        }

        public void TopWithItem(InteractableItem item)
        {
            pickedItem = item;
            ShowCarrying();
            item.AttachToPickupPoint(itemPositionWhenPickedUp);
        }

        public void RemoveItem(InteractableItem item)
        {
            pickedItem = null;
            ShowIdle();
        }

        public void PlayDeathAnimation()
        {
            Debug.Log("Player Died!");
        }

        public void PlayWinAnimation()
        {
            Debug.Log("Player Won!");
        }
        
        public override Tween MoveTo(Vector2Int newIndex, Vector3 newPos)
        {
            currentMainIndex = newIndex;
            return TweenGlobalMovePosition(newPos);
        }

        public override Tween JumpTo(Vector2Int newIndex, Vector3 newPos)
        {
            currentMainIndex = newIndex;
            return TweenGlobalJumpPosition(newPos);
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
            
            var verticalDistance = Mathf.Abs(currentPosition.y - newGlobalPosition.y);

            if (verticalDistance < 10)
            {
                var duration = 0.1f + distance * 0.0008f;
                var middlePosition = (currentPosition + newGlobalPosition) / 2;
                var offSetMiddlePosition = middlePosition + Mathf.Max(0, 40 - verticalDistance) * Vector3.up;
                _moveTween = DOTween.Sequence()
                    .Append(transform.DOMove(offSetMiddlePosition, duration / 2).SetEase(Ease.InOutQuad))
                    .Append(transform.DOMove(newGlobalPosition, duration / 2).SetEase(Ease.InOutQuad));
            }
            else
            {
                var duration = 0.1f + distance * 0.0005f;
                _moveTween = transform.DOMove(newGlobalPosition, duration).SetEase(Ease.OutQuad);
            }
            
            return _moveTween;
        }
    }
}