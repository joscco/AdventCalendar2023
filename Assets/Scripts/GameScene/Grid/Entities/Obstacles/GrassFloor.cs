using System;
using DG.Tweening;
using GameScene.Grid.Entities.Shared;
using UnityEngine;

namespace GameScene.Grid.Entities.Obstacles
{
    public class GrassFloor : GridEntity
    {
        private void Start()
        {
            InstantHide();
        }

        public void Show(float delay)
        {
            transform.DOScale(1, 0.3f)
                .SetEase(Ease.OutBack)
                .SetDelay(delay);
        }

        public void InstantHide()
        {
            transform.localScale = Vector3.zero;
        }
    }
}