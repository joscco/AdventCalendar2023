using Code;
using Code.GameScene.UI;

namespace GameScene.UI
{
    public class GameScreenBackButton : ScalingButton
    {
        public override void OnClick()
        {
            if (!SceneTransitionManager.Get().IsInTransition())
            {
                SceneTransitionManager.Get().TransitionTo("LevelChooserScene");
            }
        }
    }
}
