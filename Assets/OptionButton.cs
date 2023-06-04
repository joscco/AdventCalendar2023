using System.Collections;
using System.Collections.Generic;
using Code.GameScene.UI;
using SceneManagement;
using UnityEngine;

public class OptionButton : ScalingButton
{

    public override void OnClick()
    {
        GameSceneHeart.Get().ToggleOptionScreen();
    }

    public override bool IsEnabled()
    {
        return true;
    }
}
