using System;
using DG.Tweening;
using GameScene.PlayerControl;
using Levels.WizardLevel;
using UnityEngine;

namespace General.Grid
{
    public class PickableItem : MonoBehaviour
    {
        private Tween _moveTween;

        private void SetParent(Transform newParent)
        {
            transform.parent = newParent;
        }

        public void AttachToPickupPoint(Transform newParent)
        {
            SetParent(newParent);
            LocalMoveTo(Vector2.zero);
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
    }
}