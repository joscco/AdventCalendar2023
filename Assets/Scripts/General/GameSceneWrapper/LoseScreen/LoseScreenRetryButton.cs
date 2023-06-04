using Code.GameScene.UI;
using SceneManagement;

namespace General.LoseScreen
{
    public class LoseScreenRetryButton : ScalingButton
    {
        public override void OnClick()
        {
            SceneTransitionManager.Get().ReloadCurrentScene();
        }

        public override bool IsEnabled()
        {
            return true;
        }
    }
}