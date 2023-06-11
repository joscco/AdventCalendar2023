using DG.Tweening;
using UnityEngine;

namespace Levels.WizardLevel
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class WizardMachineLaser : MonoBehaviour
    {
        private Tween _scaleTween;

        public void BlendOut()
        {
            DoSetRendererSize(0f, 0.4f);
        }

        public void BlendIn()
        {
            DoSetRendererSize(1f, 0.4f);
        }

        public void BlendOutInstantly()
        {
            _scaleTween?.Kill();
            transform.localScale = new Vector3(transform.localScale.x, 0, 1);
        }

        private void DoSetRendererSize(float endValue, float duration)
        {
            _scaleTween?.Kill();
            transform.DOScaleY(endValue, duration)
                .SetEase(Ease.InOutQuad);
        }
    }
}