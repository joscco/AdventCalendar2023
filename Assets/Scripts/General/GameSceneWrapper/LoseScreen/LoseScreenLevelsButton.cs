using Code.GameScene.UI;
using SceneManagement;

namespace General.LoseScreen
{
    public class LoseScreenLevelsButton : ScalingButton
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