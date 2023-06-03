using Code;
using Code.GameScene.UI;
using General;
using SceneManagement;

namespace GameScene.WinScreen
{
    public class WinScreenLevelsButton : ScalingButton
    {
        public override void OnClick()
        {
            if (!SceneTransitionManager.Get().IsInTransition())
            {
                SceneTransitionManager.Get().TransitionToScene(SceneReference.MENU_LEVEL);
            }
        }

        public override bool IsEnabled()
        {
            return true;
        }
    }
}
