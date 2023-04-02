using System;
using DG.Tweening;
using UnityEngine;

namespace Code.GameScene.UI
{
    public class ProgressBar : MonoBehaviour
    {
        private int _fillBarWidth;
        private int _fillBarHeight;
        [SerializeField] private SpriteRenderer fillBar;
        [SerializeField] private SpriteRenderer outerBar;

        private void Start()
        {
            var fillBarSprite = fillBar.sprite;
            _fillBarWidth = fillBarSprite.texture.width;
            _fillBarHeight = fillBarSprite.texture.height;
        }

        private void SetSteps(int numberOfSteps)
        {
            
        }

        public Tween SetPercentFinished(float value)
        {
            float sanitizedPercent = Math.Clamp(value, 0f, 1f);
            var newSize = new Vector2(_fillBarWidth * sanitizedPercent, _fillBarHeight);
            return DOTween.To(() => fillBar.size,
                    (val) => { fillBar.size = val; },
                    newSize,
                    0.1f)
                .SetEase(Ease.InOutBack);
        }
    }
}
