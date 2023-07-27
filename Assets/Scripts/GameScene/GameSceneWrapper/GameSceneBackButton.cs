using Code.GameScene.UI;
using SceneManagement;

namespace GameScene.UI
{
    public class GameSceneBackButton : ScalingButton
    {
        protected override void OnClick()
        {
            SceneTransitionManager.Get().TransitionToNonLevelScene("LevelChoosingScene");
        }

        protected override bool IsEnabled()
        {
            return !SceneTransitionManager.Get().IsInTransition();
        }
    }
}