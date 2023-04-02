using DG.Tweening;
using UnityEngine;

namespace Code.GameScene.UI
{
    public abstract class ScalingButton: MonoBehaviour
    {
        public const float ScaleTimeInSeconds = 0.5f;
        public float MaxScale = 1.2f;

        private void OnMouseEnter()
        {
            transform.DOScale(MaxScale, ScaleTimeInSeconds).SetEase(Ease.OutBack);
        }

        private void OnMouseExit()
        {
            transform.DOScale(1f, ScaleTimeInSeconds).SetEase(Ease.OutBack);
        }

        private void OnMouseUp()
        {
            OnClick();
        }

        public abstract void OnClick();
    }
}