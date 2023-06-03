using Code;
using Code.GameScene.UI;
using General;
using SceneManagement;

namespace GameScene.WinScreen
{
    public class WinScreenNextButton : ScalingButton
    {
        public override void OnClick()
        {
            if (!SceneTransitionManager.Get().IsInTransition())
            {
                SceneTransitionManager.Get().TransitionToNextLevel();
            }
        }
        public override bool IsEnabled()
        {
            return true;
        }
    }
}
