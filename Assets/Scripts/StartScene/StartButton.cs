using Code.GameScene.UI;
using SceneManagement;

namespace StartScene
{
    public class StartButton : ScalingButton
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