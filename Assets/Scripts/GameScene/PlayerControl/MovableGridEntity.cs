using DG.Tweening;
using UnityEngine;

namespace GameScene.PlayerControl
{
    public class MovableGridEntity : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        protected Vector2Int currentIndex;
        private Tween _moveTween;
        private Tween _scaleTween;

        private void OnDestroy()
        {
            _moveTween?.Kill();
            _scaleTween?.Kill();
        }

        public void StartShaking()
        {
            _scaleTween = transform.DOScale(new Vector2(0.95f, 1.05f), 0.5f)
                .SetEase(Ease.InOutQuad)
                .SetLoops(-1, LoopType.Yoyo);
        }

        public void InstantUpdatePosition(Vector2Int newIndex, Vector2 newPos)
        {
            currentIndex = newIndex;
            _moveTween?.Kill();
            transform.position = newPos;
        }

        public void UpdatePosition(Vector2Int newIndex, Vector2 newPos)
        {
            if (newIndex.x < currentIndex.x)
            {
                spriteRenderer.transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (newIndex.x > currentIndex.x)
            {
                spriteRenderer.transform.localScale= new Vector3(1, 1, 1);
            }
            currentIndex = newIndex;
            _moveTween?.Kill();
            _moveTween = transform.DOMove(newPos, 0.15f)
                .SetEase(Ease.InOutCirc);
        }

        public Vector2Int GetIndex()
        {
            return currentIndex;
        }
        
        public void BlendOutInstantly()
        {
            transform.localScale = Vector3.zero;
        }
        
        public Tween BlendOut()
        {
            _scaleTween?.Kill();
            var blendOutTween = transform.DOScale(0f, 0.2f)
                .SetEase(Ease.InBack);
            _scaleTween = blendOutTween;
            return blendOutTween;
        }
        
        public Tween BlendIn()
        {
            _scaleTween?.Kill();
            var blendInTween = transform.DOScale(1f, 0.2f)
                .SetEase(Ease.OutBack);
            _scaleTween = blendInTween;
            return blendInTween;
        }
    }
}