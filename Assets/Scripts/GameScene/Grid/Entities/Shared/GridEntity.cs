using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace GameScene.Grid.Entities.Shared
{
    public class GridEntity : MonoBehaviour
    {
        [SerializeField] protected Vector2Int currentMainIndex;
        [SerializeField] private List<Vector2Int> relativeIndicesIncluded = new() { Vector2Int.zero };
        private Tween _scaleTween;

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

        public List<Vector2Int> GetCoveredIndices()
        {
            return relativeIndicesIncluded.Select(index => currentMainIndex + index).ToList();
        }

        public List<Vector2Int> GetCoveredIndicesIfMainIndexWas(Vector2Int potentialMainIndex)
        {
            return relativeIndicesIncluded.Select(index => potentialMainIndex + index).ToList();
        }

        public bool IsSingleIndex()
        {
            return relativeIndicesIncluded.Count == 1;
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