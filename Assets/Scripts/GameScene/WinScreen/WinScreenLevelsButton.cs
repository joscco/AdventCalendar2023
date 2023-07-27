using Code.GameScene.UI;
using SceneManagement;

namespace GameScene.WinScreen
{
    public class WinScreenLevelsButton : ScalingButton
    {
        protected override void OnClick()
        {
            SceneTransitionManager.Get().TransitionToNonLevelScene("LevelChoosingScene");
        }

        protected override bool IsEnabled()
        {
            return true;
        }
    }
}