using System.Collections;
using System.Collections.Generic;
using Code.GameScene.UI;
using SceneManagement;
using UnityEngine;

public class LevelChoosingSceneBackButton : ScalingButton
{
    public override void OnClick()
    {
        SceneTransitionManager.Get().TransitionToScene(SceneReference.START);
    }

    public override bool IsEnabled()
    {
        return true;
    }
}