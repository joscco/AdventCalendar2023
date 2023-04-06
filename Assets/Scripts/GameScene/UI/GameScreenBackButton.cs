using System.Collections;
using System.Collections.Generic;
using Code;
using Code.GameScene.UI;
using UnityEngine;

public class GameScreenBackButton : ScalingButton
{
    public override void OnClick()
    {
        if (!SceneTransitionManager.Get().IsInTransition())
        {
            SceneTransitionManager.Get().TransitionTo("LevelChooserScene");
        }
    }
}
