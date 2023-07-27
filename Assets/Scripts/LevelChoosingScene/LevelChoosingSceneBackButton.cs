using Code.GameScene.UI;
using SceneManagement;

public class LevelChoosingSceneBackButton : ScalingButton
{
    protected override void OnClick()
    {
        SceneTransitionManager.Get().TransitionToNonLevelScene("StartScene");
    }

    protected override bool IsEnabled()
    {
        return true;
    }
}