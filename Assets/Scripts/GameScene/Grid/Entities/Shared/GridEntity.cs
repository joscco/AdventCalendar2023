using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

namespace GameScene.Grid.Entities.Shared
{
    public class GridEntity : MonoBehaviour
    {
        [SerializeField] protected Vector2Int currentMainIndex;
        [SerializeField] private List<Vector2Int> relativeIndicesIncluded = new() { Vector2Int.zero };
        private Tween _scaleTween;

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

        protected Tween scaleTween;

        protected void OnDestroy()
        {
            scaleTween?.Kill();
        }

        public Tween BlendIn()
        {
            scaleTween?.Kill();
            var blendInTween = transform.transform.DOScale(1f, 0.4f)
                .SetEase(Ease.OutBack);
            scaleTween = blendInTween;
            return blendInTween;
        }

        public void BlendOutInstantly()
        {
            transform.localScale = Vector3.zero;
        }

        public Tween BlendOut()
        {
            scaleTween?.Kill();
            var blendOutTween = transform.transform.DOScale(0f, 0.4f)
                .SetEase(Ease.InBack);
            scaleTween = blendOutTween;
            return blendOutTween;
        }
    }
}