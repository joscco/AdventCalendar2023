using Code;
using Code.GameScene.UI;

namespace GameScene.WinScreen
{
    public class WinScreenLevelsButton : ScalingButton
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
