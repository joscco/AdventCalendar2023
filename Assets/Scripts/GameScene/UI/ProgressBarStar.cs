using DG.Tweening;
using UnityEngine;

namespace GameScene.UI
{
    public class ProgressBarStar: MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite emptyStarSprite;
        [SerializeField] private Sprite fullStarSprite;

        private float _percentageNeeded;
        private StarState _state;

        private enum StarState
        {
            ACHIEVED,
            NOT_ACHIEVED
        }
        
        private void Start()
        {
            spriteRenderer.sprite = emptyStarSprite;
            _state = StarState.NOT_ACHIEVED;
        }

        public void SetPercentageNeeded(float percentageNeeded)
        {
            _percentageNeeded = percentageNeeded;
        }

        public void UpdateForPercentage(float achievedPercentage)
        {
            if (achievedPercentage >= _percentageNeeded)
            {
                if (_state != StarState.ACHIEVED)
                {
                    ScaleUpAndDown();
                }
                spriteRenderer.sprite = fullStarSprite;
                _state = StarState.ACHIEVED;
            }
            else
            {
                if (_state != StarState.NOT_ACHIEVED)
                {
                    ScaleUpAndDown();
                }
                spriteRenderer.sprite = emptyStarSprite;
                _state = StarState.NOT_ACHIEVED;
            }
        }

        private void ScaleUpAndDown()
        {
            var sequence = DOTween.Sequence();
            sequence.Insert(0, spriteRenderer.transform.DOScale(1.05f, 0.2f).SetEase(Ease.OutQuad));
            sequence.Insert(0.2f,spriteRenderer.transform.DOScale(1f, 0.2f).SetEase(Ease.OutQuad));
            sequence.Play();
        }
    }
}