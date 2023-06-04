using Code.GameScene.UI;
using SceneManagement;

namespace General.OptionScreen
{
    public class OptionScreenCancelButton : ScalingButton
    {
        public override void OnClick()
        {
            GameSceneHeart.Get().BlendOutOptionScreen();
        }

        public override bool IsEnabled()
        {
            return true;
        }
    }
}