using DG.Tweening;
using UnityEngine;

namespace GameScene.Grid.Entities.Shared
{
    public class GridEntity : MonoBehaviour
    {
        private Tween _scaleTween;
        protected Vector2Int currentMainIndex;
        protected int currentOrder;

        protected virtual void OnDestroy()
        {
            _scaleTween?.Kill();
        }

        public void SetIndicesAndPosition(Vector2Int newMainIndex, Vector3 newPosition)
        {
            currentMainIndex = newMainIndex;
            transform.position = newPosition;
        }

        public Vector2Int GetMainIndex()
        {
            return currentMainIndex;
        }

        public Tween BlendIn()
        {
            _scaleTween?.Kill();
            var blendInTween = transform.transform.DOScale(1f, 0.4f)
                .SetEase(Ease.OutBack);
            _scaleTween = blendInTween;
            return blendInTween;
        }

        public Tween BlendOut()
        {
            _scaleTween?.Kill();
            var blendOutTween = transform.transform.DOScale(0f, 0.4f)
                .SetEase(Ease.InBack);
            _scaleTween = blendOutTween;
            return blendOutTween;
        }
    }
}