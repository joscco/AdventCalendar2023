using DG.Tweening;
using GameScene.Grid.Entities.Shared;
using UnityEngine;

namespace GameScene.Grid.Entities.Player
{
    public class InfiniteShaker : MonoBehaviour
    {
        [SerializeField] private float endScaleX = 0.95f;
        [SerializeField] private float endScaleY = 1.05f;
        [SerializeField] private float duration = 0.5f;
        
        private Tween _spriteScaleTween;

        private void Start()
        {
            StartShaking();
        }

        public void StartShaking()
        {
            _spriteScaleTween = transform.DOScale(new Vector3(endScaleX, endScaleY, 1), duration)
                .SetEase(Ease.InOutQuad)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}
