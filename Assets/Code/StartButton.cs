using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code
{
    public class StartButton : MonoBehaviour
    {
        public String mainGameSceneName = "GameScene";

        public const float ScaleTimeInSeconds = 0.5f;
        public const float MaxScale = 1.2f;

        private void OnMouseEnter()
        {
            transform.DOScale(MaxScale, ScaleTimeInSeconds).SetEase(Ease.OutBack);
        }

        private void OnMouseExit()
        {
            transform.DOScale(1f, ScaleTimeInSeconds).SetEase(Ease.OutBack);
        }

        private void OnMouseUp()
        {
            SceneTransitionManager.get().TransitionTo(mainGameSceneName);
        }
    }
}
