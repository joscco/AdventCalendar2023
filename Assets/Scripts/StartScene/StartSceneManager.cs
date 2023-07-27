using DG.Tweening;
using General;
using UnityEngine;

namespace StartScene
{
    public class StartSceneManager : MonoBehaviour
    {
        private const KeyCode OptionScreenKey = KeyCode.O;
        private const KeyCode StartKey = KeyCode.Space;

        public TitleAnimation titleAnimation;
        public StartButton startButton;
        public OptionButton optionButton;

        private void Start()
        {
            DOVirtual.DelayedCall(0.5f, () => titleAnimation.FadeIn());
        }

        private void Update()
        {
            if (Input.GetKeyDown(OptionScreenKey))
            {
                optionButton.Activate();
                return;
            }
        
            if (Input.GetKeyDown(StartKey))
            {
                startButton.Activate();
            }
        }
    }
}