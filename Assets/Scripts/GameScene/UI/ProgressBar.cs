using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace GameScene.UI
{
    public class ProgressBar : MonoBehaviour
    {
        private int _fillBarWidth;
        private int _fillBarHeight;
        
        [SerializeField] private SpriteRenderer fillBar;
        [SerializeField] private ProgressBarStar starPrefab;

        private List<ProgressBarStar> _stars;

        private void Awake()
        {
            _stars = new List<ProgressBarStar>();
            
            var fillBarSprite = fillBar.sprite;
            _fillBarWidth = fillBarSprite.texture.width;
            _fillBarHeight = fillBarSprite.texture.height;
        }

        public Tween SetPercentFinished(float value)
        {
            float sanitizedPercent = Math.Clamp(value, 0f, 1f);
            
            foreach (var progressBarStar in _stars)
            {
                progressBarStar.UpdateForPercentage(sanitizedPercent);
            }
            
            
            var newSize = new Vector2(_fillBarWidth * sanitizedPercent, _fillBarHeight);
            return DOTween.To(() => fillBar.size,
                    (val) => { fillBar.size = val; },
                    newSize,
                    0.3f)
                .SetEase(Ease.OutQuad);
        }

        public void InstantSetPercentFinished(float value)
        {
            float sanitizedPercent = Math.Clamp(value, 0f, 1f);
            var newSize = new Vector2(_fillBarWidth * sanitizedPercent, _fillBarHeight);
            fillBar.size = newSize;
        }

        public void SetStar(float percentageNeeded)
        {
            var newStar = Instantiate(starPrefab, fillBar.transform);
            newStar.transform.position = fillBar.transform.position + new Vector3(percentageNeeded * _fillBarWidth, 0, 0);
            newStar.SetPercentageNeeded(percentageNeeded);
            _stars.Add(newStar);
        }
    }
}
