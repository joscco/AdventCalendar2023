using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

namespace Code.GameScene.UI
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class ScalingButton : MonoBehaviour
    {
        private const float ScaleTimeInSeconds = 0.3f;
        private const float ClickScaleTimeInSeconds = 0.15f;
        private const float ScaleWhenSelected = 1.15f;
        private const float ClickScale = 1.3f;

        private Tween _scaleTween;
        protected bool _selected;

        private void OnDestroy()
        {
            _scaleTween?.Kill();
        }

        private void OnMouseEnter()
        {
            if (IsEnabled())
            {
                Select();
            }
        }

        private void OnMouseExit()
        {
            if (IsEnabled())
            {
                Deselect();
            }
        }

        private void OnMouseUp()
        {
            if (IsEnabled())
            {
                OnClick();
            }
        }

        protected void ScaleUpOnClick()
        {
            _scaleTween?.Kill();
            _scaleTween = transform.DOScale(ClickScale, ClickScaleTimeInSeconds).SetEase(Ease.OutBack);
        }

        protected void ScaleDownAfterClick()
        {
            _scaleTween?.Kill();
            _scaleTween = transform.DOScale(_selected ? ScaleWhenSelected : 1f, ClickScaleTimeInSeconds).SetEase(Ease.OutBack);
        }

        private void ScaleUp()
        {
            _scaleTween?.Kill();
            if (null != transform)
            {
                _scaleTween = transform.DOScale(ScaleWhenSelected, ScaleTimeInSeconds).SetEase(Ease.OutBack);
            }
        }

        private void ScaleDown()
        {
            _scaleTween?.Kill();
            if (null != transform)
            {
                _scaleTween = transform.DOScale(1f, ScaleTimeInSeconds).SetEase(Ease.OutBack);
            }
        }
        
        /*** Scale Up and Down periodically until the Button is Selected */
        public void StartWobbling()
        {
            _scaleTween?.Kill();
            if (null != transform)
            {
                _scaleTween = DOTween.Sequence()
                    .Append(transform.DOScale(1.05f, 0.5f).SetEase(Ease.OutQuad))
                    .Append(transform.DOScale(1f, 0.5f).SetEase(Ease.InQuad))
                    .SetLoops(-1)
                    .Play();
            }
        }

        public abstract void OnClick();

        public abstract bool IsEnabled();

        public void Activate()
        {
            if (IsEnabled())
            {
                OnClick();
            }
        }

        public virtual void Select()
        {
            _selected = true;
            ScaleUp();
        }

        public virtual void Deselect()
        {
            _selected = false;
            ScaleDown();
        }
    }
}