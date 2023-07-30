using UI;

namespace GameScene.Options
{
    public abstract class OnOffButton : ScalingButton
    {
        public abstract void SetOn();

        public abstract void SetOff();
    }
}