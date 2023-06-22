using DG.Tweening;
using GameScene.Grid.Entities.Shared;
using UnityEngine;

namespace GameScene.Grid.Entities.Player
{
    public class PlayerRenderer : EntityRenderer
    {
        [SerializeField] private Sprite spriteWhenCarrying;
        [SerializeField] private Sprite spriteWhenNormal;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private Tween _spriteScaleTween;

        private void Start()
        {
            StartShaking();
        }

        public void ShowCarrying()
        {
            spriteRenderer.sprite = spriteWhenCarrying;
        }
    
        public void ShowIdle()
        {
            spriteRenderer.sprite = spriteWhenNormal;
        }
    
        public void StartShaking()
        {
            _spriteScaleTween = transform.DOScale(new Vector2(0.95f, 1.05f), 0.5f)
                .SetEase(Ease.InOutQuad)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}
