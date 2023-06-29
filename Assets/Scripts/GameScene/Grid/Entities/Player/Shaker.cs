using DG.Tweening;
using GameScene.Grid.Entities.Shared;
using UnityEngine;

namespace GameScene.Grid.Entities.Player
{
    public class Shaker : MonoBehaviour
    {

        private Tween _spriteScaleTween;

        private void Start()
        {
            StartShaking();
        }

        public void StartShaking()
        {
            _spriteScaleTween = transform.DOScale(new Vector3(0.95f, 1.05f, 1), 0.5f)
                .SetEase(Ease.InOutQuad)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}
