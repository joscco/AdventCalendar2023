using Code.GameScene.UI;
using SceneManagement;

namespace General.WinScreen
{
    public class WinScreenNextButton : ScalingButton
    {
        public override void OnClick()
        {
            SceneTransitionManager.Get().TransitionToNextLevel();
        }

        public override bool IsEnabled()
        {
            return true;
        }
    }
}