using System;
using Code.GameScene.UI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code
{
    public class StartButton : ScalingButton
    {
        public String levelChooserSceneName = "LevelChooserScene";
        
        public override void OnClick()
        {
            SceneTransitionManager.Get().TransitionTo(levelChooserSceneName);
        }
    }
}
