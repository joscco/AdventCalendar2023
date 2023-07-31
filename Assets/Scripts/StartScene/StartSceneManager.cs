using DG.Tweening;
using GameScene.Options;
using General;
using SceneManagement;
using UI;
using UnityEngine;

namespace StartScene
{
    public class StartSceneManager : MonoBehaviour
    {
        private const KeyCode OptionScreenKey = KeyCode.P;
        private const KeyCode StartKey = KeyCode.Space;

        public TitleAnimation titleAnimation;
        public ScalingButton startButton;
        public ScalingButton optionScreenButton;

        private void Start()
        {
            DOVirtual.DelayedCall(0.5f, () => titleAnimation.FadeIn());
            
            // Touch Control
            optionScreenButton.OnButtonHover += () => optionScreenButton.ScaleUp();
            optionScreenButton.OnButtonExit += () => optionScreenButton.ScaleDown();
            optionScreenButton.OnButtonClick += ActivateOptionButton;
            startButton.OnButtonHover += () => startButton.ScaleUp();
            startButton.OnButtonExit += () => startButton.ScaleDown();
            startButton.OnButtonClick += ActivateStartButton;
            
            startButton.StartWobbling();
        }

        private void Update()
        {
            if (Input.GetKeyDown(OptionScreenKey))
            {
                ActivateOptionButton();
                return;
            }
        
            if (Input.GetKeyDown(StartKey))
            {
                ActivateStartButton();
            }
            
            // Updating Option Screen if necessary
            if (OptionScreen.instance.IsShowing())
            {
                OptionScreen.instance.HandleUpdate();
            }
        }

        private void ActivateStartButton()
        {
            SceneTransitionManager.Get().TransitionToNonLevelScene("LevelChoosingScene");
            startButton.ScaleUpThenDown();
        }

        private void ActivateOptionButton()
        {
            OptionScreen.instance.Toggle();
            optionScreenButton.ScaleUpThenDown();
        }
    }
}