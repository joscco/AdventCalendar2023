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
                    ScaleUpDownAndGoUp();
                }
                _state = StarState.ACHIEVED;
            }
            else
            {
                if (_state != StarState.NOT_ACHIEVED)
                {
                    ScaleUpDownAndGoDown();
                }
                _state = StarState.NOT_ACHIEVED;
            }
        }

        private void ScaleUpDownAndGoUp()
        {
            var sequence = DOTween.Sequence();
            sequence.Insert(0, spriteRenderer.transform.DOScale(0.5f, 0.2f).SetEase(Ease.InOutQuad));
            sequence.InsertCallback(0.2f, () =>
            {
                spriteRenderer.sprite = fullStarSprite;
            });
            sequence.Insert(0.2f,spriteRenderer.transform.DOScale(1f, 0.2f).SetEase(Ease.InOutQuad));
            sequence.Insert(0.4f,spriteRenderer.transform.DOLocalMoveY(35f, 0.2f).SetEase(Ease.InOutQuad));
            sequence.Play();
        }
        
        private void ScaleUpDownAndGoDown()
        {
            var sequence = DOTween.Sequence();
            sequence.Insert(0, spriteRenderer.transform.DOScale(0.5f, 0.2f).SetEase(Ease.InOutQuad));
            sequence.InsertCallback(0.2f, () =>
            {
                spriteRenderer.sprite = emptyStarSprite;
            });
            sequence.Insert(0.2f,spriteRenderer.transform.DOScale(1f, 0.2f).SetEase(Ease.InOutQuad));
            sequence.Insert(0.4f,spriteRenderer.transform.DOLocalMoveY(0f, 0.2f).SetEase(Ease.InOutQuad));
            sequence.Play();
        }
    }
}