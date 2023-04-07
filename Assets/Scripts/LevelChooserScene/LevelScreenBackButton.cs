using Code;
using Code.GameScene.UI;

namespace LevelChooserScene
{
    public class LevelScreenBackButton : ScalingButton
    {
        public override void OnClick()
        {
            if (!SceneTransitionManager.Get().IsInTransition())
            {
                SceneTransitionManager.Get().TransitionTo("StartScene");
            }
        }
    }
}
