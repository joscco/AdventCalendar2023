using Code.GameScene.UI;
using SceneManagement;

namespace General.WinScreen
{
    public class WinScreenLevelsButton : ScalingButton
    {
        public override void OnClick()
        {
            SceneTransitionManager.Get().TransitionToScene(SceneReference.MENU_LEVEL);
        }

        public override bool IsEnabled()
        {
            return true;
        }
    }
}