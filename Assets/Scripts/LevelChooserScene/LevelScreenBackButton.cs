using System.Collections;
using System.Collections.Generic;
using Code;
using Code.GameScene.UI;
using UnityEngine;

public class LevelScreenBackButton : ScalingButton
{
    public override void OnClick()
    {
        if (!SceneTransitionManager.Get().IsInTransition())
        {
            SceneTransitionManager.Get().TransitionTo("StartScene");
        }
    }
}
