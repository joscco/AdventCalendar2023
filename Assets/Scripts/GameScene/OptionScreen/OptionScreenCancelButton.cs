using Code.GameScene.UI;
using SceneManagement;

namespace General.OptionScreen
{
    public class OptionScreenCancelButton : ScalingButton
    {
        protected override void OnClick()
        {
            GameSceneHeart.Get().BlendOutOptionScreen();
        }

        protected override bool IsEnabled()
        {
            return true;
        }
    }
}