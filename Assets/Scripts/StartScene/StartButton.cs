using Code.GameScene.UI;
using SceneManagement;

namespace StartScene
{
    public class StartButton : ScalingButton
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