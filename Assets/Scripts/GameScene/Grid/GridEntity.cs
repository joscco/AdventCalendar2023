using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace General.Grid
{
    public class GridEntity : MonoBehaviour
    {
        [SerializeField] protected List<SpriteRenderer> spriteRenderers;
        [SerializeField] protected Transform spriteTransform;
        [SerializeField] protected Vector2Int currentMainIndex;
        [SerializeField] private List<Vector2Int> relativeIndicesIncluded = new() { Vector2Int.zero };

        protected Tween _spriteScaleTween;

        protected void OnDestroy()
        {
            _spriteScaleTween?.Kill();
        }

        public void SetIndicesAndPosition(Vector2Int newMainIndex, Vector2 newPosition)
        {
            currentMainIndex = newMainIndex;
            transform.position = newPosition;
        }

        public Vector2Int GetMainIndex()
        {
            return currentMainIndex;
        }

        public List<Vector2Int> GetCoveredIndices()
        {
            return relativeIndicesIncluded.Select(index => currentMainIndex + index).ToList();
        }

        public List<Vector2Int> GetCoveredIndicesWhenMainIndexWas(Vector2Int potentialMainIndex)
        {
            return relativeIndicesIncluded.Select(index => potentialMainIndex + index).ToList();
        }

        public void StartShaking()
        {
            _spriteScaleTween = spriteTransform.transform.DOScale(new Vector2(0.95f, 1.05f), 0.5f)
                .SetEase(Ease.InOutQuad)
                .SetLoops(-1, LoopType.Yoyo);
        }

        public Tween BlendIn()
        {
            _spriteScaleTween?.Kill();
            var blendInTween = spriteTransform.transform.DOScale(1f, 0.2f)
                .SetEase(Ease.OutBack);
            _spriteScaleTween = blendInTween;
            return blendInTween;
        }

        public void BlendOutInstantly()
        {
            spriteTransform.transform.localScale = Vector3.zero;
        }

        public Tween BlendOut()
        {
            _spriteScaleTween?.Kill();
            var blendOutTween = spriteTransform.transform.DOScale(0f, 0.2f)
                .SetEase(Ease.InBack);
            _spriteScaleTween = blendOutTween;
            return blendOutTween;
        }
    }
}