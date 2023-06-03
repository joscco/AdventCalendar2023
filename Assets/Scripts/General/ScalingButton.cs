using DG.Tweening;
using UnityEngine;

namespace Code.GameScene.UI
{
    public abstract class ScalingButton : MonoBehaviour
    {
        private const float ScaleTimeInSeconds = 0.3f;
        private const float ClickScaleTimeInSeconds = 0.15f;
        private const float HoverScale = 1.15f;
        private const float ClickScale = 1.3f;

        private bool _hovering;

        public virtual void Start()
        {
            if (!GetComponent<Collider2D>())
            {
                Debug.LogError("No collider present!");
            }
        }

        private void OnMouseEnter()
        {
            _hovering = true;
            if (IsEnabled())
            {
                ScaleUp();
            }
        }

        private void OnMouseExit()
        {
            _hovering = false;
            if (IsEnabled())
            {
                ScaleDown();
            }
        }

        private void OnMouseUp()
        {
            if (IsEnabled())
            {
                OnClick();
            }
        }

        protected Tween ScaleUpOnClick()
        {
            return transform.DOScale(ClickScale, ClickScaleTimeInSeconds).SetEase(Ease.OutBack);
        }

        protected Tween ScaleDownAfterClick()
        {
            return transform.DOScale(_hovering ? HoverScale : 1f, ClickScaleTimeInSeconds).SetEase(Ease.OutBack);
        }

        private Tween ScaleUp()
        {
            return transform.DOScale(HoverScale, ScaleTimeInSeconds).SetEase(Ease.OutBack);
        }

        private Tween ScaleDown()
        {
            return transform.DOScale(1f, ScaleTimeInSeconds).SetEase(Ease.OutBack);
        }

        public abstract void OnClick();

        public abstract bool IsEnabled();

        public void Trigger()
        {
            if (IsEnabled())
            {
                OnClick();
            }
        }

        public void OnSetActive()
        {
            ScaleUp();
        }

        public void OnSetDeactive()
        {
            ScaleDown();
        }

    }
}