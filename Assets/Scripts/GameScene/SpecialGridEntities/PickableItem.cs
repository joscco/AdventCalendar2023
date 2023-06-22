using System.Collections.Generic;
using DG.Tweening;
using GameScene.Grid.Entities.Shared;
using GameScene.SpecialGridEntities.PickPoints;
using UnityEngine;

namespace General.Grid
{
    public class PickableItem : MonoBehaviour
    {
        [SerializeField] private PickableItemType type;
        [SerializeField] private EntityRenderer pickableRenderer;
        private Tween _moveTween;

        public void AttachToPickupPoint(Transform newParent)
        {
            SetParent(newParent);
            LocalMoveTo(Vector2.zero);
        }

        public PickableItemType GetItemType()
        {
            return type;
        }

        private void SetParent(Transform newParent)
        {
            transform.parent = newParent;
        }

        private void StopMoving()
        {
            _moveTween?.Kill();
        }

        private Tween LocalMoveTo(Vector2 position)
        {
            StopMoving();
            _moveTween = transform.DOLocalMove(position, 0.15f)
                .SetEase(Ease.InOutCirc);
            return _moveTween;
        }

        public void SetSortingOrder(int mainOrder)
        {
            pickableRenderer.SetSortingOrder(mainOrder);
        }
    }
}