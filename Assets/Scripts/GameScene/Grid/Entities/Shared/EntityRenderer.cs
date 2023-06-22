using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace GameScene.Grid.Entities.Shared
{
    public class EntityRenderer : MonoBehaviour
    {
        [SerializeField] private List<RelativeOrderSpriteRendererPair> relativeSpriteRendererPairs;
        protected Tween scaleTween;

        protected void OnDestroy()
        {
            scaleTween?.Kill();
        }

        public Tween BlendIn()
        {
            scaleTween?.Kill();
            var blendInTween = transform.transform.DOScale(1f, 0.2f)
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
            var blendOutTween = transform.transform.DOScale(0f, 0.2f)
                .SetEase(Ease.InBack);
            scaleTween = blendOutTween;
            return blendOutTween;
        }

        public void SetSortingOrder(int mainOrder)
        {
            foreach (var relativeOrderPair in relativeSpriteRendererPairs)
            {
                relativeOrderPair.renderer.sortingOrder = mainOrder * 100 + relativeOrderPair.relativeOrder;
            }
        }

        [Serializable]
        public class RelativeOrderSpriteRendererPair
        {
            public SpriteRenderer renderer;
            public int relativeOrder;
            
            public RelativeOrderSpriteRendererPair(SpriteRenderer renderer, int relativeOrder){
                this.renderer = renderer;
                this.relativeOrder = relativeOrder;
            }
        }
    }
}