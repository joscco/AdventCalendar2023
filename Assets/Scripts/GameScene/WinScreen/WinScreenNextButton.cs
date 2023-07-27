using Code.GameScene.UI;
using SceneManagement;

namespace General.WinScreen
{
    public class WinScreenNextButton : ScalingButton
    {
        protected override void OnClick()
        {
            SceneTransitionManager.Get().TransitionToNextLevel();
        }

        protected override bool IsEnabled()
        {
            return true;
        }
    }
}