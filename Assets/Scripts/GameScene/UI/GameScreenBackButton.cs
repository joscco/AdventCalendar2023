using Code.GameScene.UI;
using General;
using SceneManagement;

namespace GameScene.UI
{
    public class GameScreenBackButton : ScalingButton
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
