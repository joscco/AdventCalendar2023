using System.Collections.Generic;
using DG.Tweening;
using GameScene.Grid.Entities.Shared;
using UnityEngine;

namespace GameScene.Grid.Entities.Obstacles
{
    public class Wall : GridEntity
    {
        [SerializeField] private bool blocking = true;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private List<ParticleSystem> particleEmitters;

        public void Unblock()
        {
            blocking = false;
            particleEmitters.ForEach(emitter => emitter.Play());
            Hide();
        }

        public bool IsBlocking()
        {
            return blocking;
        }

        private void Hide()
        {
            DOTween.Sequence()
                .Append(spriteRenderer.transform.DOShakePosition(0.5f, new Vector3(10, 0, 0)))
                .Append(spriteRenderer.DOFade(0, 0.8f).SetEase(Ease.OutQuad))
                .Join(spriteRenderer.transform.DOScaleY(0.2f, 0.8f).SetEase(Ease.OutQuad));
        }
    }
}