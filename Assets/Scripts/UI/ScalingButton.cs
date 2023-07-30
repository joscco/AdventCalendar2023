using System;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(Collider2D))]
    public class ScalingButton : MonoBehaviour
    {
        private const float ScaleTimeInSeconds = 0.3f;
        private const float ClickScaleTimeInSeconds = 0.15f;
        private const float ScaleWhenSelected = 1.15f;
        private const float ClickScale = 1.3f;

        public event Action OnButtonHover;
        public event Action OnButtonExit;
        public event Action OnButtonClick;

        private Tween _scaleTween;
        protected bool hovered;

        public void ScaleUp()
        {
            _scaleTween?.Kill();
            if (null != transform)
            {
                _scaleTween = transform.DOScale(ScaleWhenSelected, ScaleTimeInSeconds).SetEase(Ease.OutBack);
            }
        }

        public void ScaleDown()
        {
            _scaleTween?.Kill();
            if (null != transform)
            {
                _scaleTween = transform.DOScale(1f, ScaleTimeInSeconds).SetEase(Ease.OutBack);
            }
        }

        public void ScaleUpThenDown()
        {
            _scaleTween?.Kill();
            _scaleTween = DOTween.Sequence()
                .Append(transform.DOScale(ClickScale, ClickScaleTimeInSeconds).SetEase(Ease.OutBack))
                .Append(transform.DOScale(hovered ? ScaleWhenSelected : 1f, ClickScaleTimeInSeconds).SetEase(Ease.OutBack));
        }
        
        /*** Scale Up and Down periodically until the Button is Selected */
        public void StartWobbling()
        {
            _scaleTween?.Kill();
            if (null != transform)
            {
                _scaleTween = DOTween.Sequence()
                    .Append(transform.DOScale(1.03f, 0.5f).SetEase(Ease.OutBack))
                    .Append(transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack))
                    .SetLoops(-1)
                    .Play();
            }
        }

        private void OnDestroy()
        {
            _scaleTween?.Kill();
        }

        protected virtual void OnMouseEnter()
        {
            hovered = true;
            OnButtonHover?.Invoke();
        }

        protected virtual void OnMouseExit()
        {
            hovered = false;
            OnButtonExit?.Invoke();
        }

        protected virtual void OnMouseUp()
        {
            OnButtonClick?.Invoke();
        }
    }
}