using System;
using Code.GameScene.UI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code
{
    public class StartButton : ScalingButton
    {
        public String mainGameSceneName = "GameScene";
        
        public override void OnClick()
        {
            SceneTransitionManager.get().TransitionTo(mainGameSceneName);
        }
    }
}
