using Code;
using Code.GameScene.UI;

namespace GameScene.WinScreen
{
    public class WinScreenNextButton : ScalingButton
    {
        public override void OnClick()
        {
            if (!SceneTransitionManager.Get().IsInTransition())
            {
                SceneTransitionManager.Get().TransitionTo("GameScene");
            }
        }
    }
}
