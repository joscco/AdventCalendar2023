using DG.Tweening;
using GameScene.Grid.Entities.Shared;
using GameScene.SpecialGridEntities.PickPoints;
using UnityEngine;

namespace GameScene.Grid.Entities.ItemInteraction
{
    public abstract class InteractableItem : MovableGridEntity, IInteractableItemBearer
    {
        [SerializeField] private Transform itemHolder;
        [SerializeField] private bool pickable;
        [SerializeField] private bool pushable;
        [SerializeField] private bool interactable = true;
        [SerializeField] private InteractableItemType type;

        private bool wasCompletedAlready;

        protected virtual void OnFirstComplete() { }
        
        protected virtual void OnEachComplete() { }

        protected virtual void OnTopWithItem(InteractableItem item) { }

        public void AttachToPickupPoint(Transform newParent)
        {
            SetParent(newParent);
            // Set index to zero just in case
            JumpTo(Vector2Int.zero, newParent.position);
        }

        public InteractableItemType GetItemType()
        {
            return type;
        }

        private void SetParent(Transform newParent)
        {
            transform.parent = newParent;
        }

        public bool IsPickable()
        {
            return pickable;
        }

        public bool IsPushable()
        {
            return pushable;
        }

        public bool IsInteractable()
        {
            return interactable;
        }
        
        protected void SetPickable(bool val)
        {
            pickable = val;
        }

        protected void SetPushable(bool val)
        {
            pushable = val;
        }

        protected void SetInteractable(bool val)
        {
            interactable = val;
        }

        public Transform GetItemHolder()
        {
            return itemHolder;
        }

        public abstract bool CanBeToppedWithItem(InteractableItem item);

        public abstract InteractableItem GetItem();

        public void TopWithItem(InteractableItem item)
        {
            OnTopWithItem(item);

            if (IsComplete())
            {
                if (!wasCompletedAlready)
                {
                    wasCompletedAlready = true;
                    OnFirstComplete();
                }
                OnEachComplete();
            }
        }

        public abstract bool IsBearingItem();

        public abstract void RemoveItem(InteractableItem item);

        public abstract bool IsComplete();
        
        public override Tween MoveTo(Vector2Int newIndex, Vector3 newPos)
        {
            SwapFaceDirectionIfNecessary(newIndex.x);
            currentMainIndex = newIndex;
            return TweenGlobalMovePosition(newPos);
        }

        public override Tween JumpTo(Vector2Int newIndex, Vector3 newPos)
        {
            SwapFaceDirectionIfNecessary(newIndex.x);
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
            _moveTween = transform.DOMove(newGlobalPosition, duration).SetEase(Ease.OutSine);
            return _moveTween;
        }

        private Tween TweenGlobalJumpPosition(Vector3 newGlobalPosition)
        {
            StopMoving();
            var currentPosition = transform.position;
            var distance = (currentPosition - new Vector3(newGlobalPosition.x, newGlobalPosition.y, currentPosition.z)).magnitude;
            var duration = 0.1f + distance * 0.0005f;
            var middlePosition = (currentPosition + newGlobalPosition) / 2;
            var offSetMiddlePosition = middlePosition + 45 * Vector3.up;
            _moveTween = DOTween.Sequence()
                .Append(transform.DOMove(offSetMiddlePosition, duration / 2).SetEase(Ease.OutSine))
                .Append(transform.DOMove(newGlobalPosition, duration / 2).SetEase(Ease.OutSine));
            return _moveTween;
        }
    }
}