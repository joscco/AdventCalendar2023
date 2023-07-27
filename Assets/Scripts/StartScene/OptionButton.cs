using Code.GameScene.UI;
using SceneManagement;

public class OptionButton : ScalingButton
{

    protected override void OnClick()
    {
        GameSceneHeart.Get().ToggleOptionScreen();
    }

    protected override bool IsEnabled()
    {
        return true;
    }
}
