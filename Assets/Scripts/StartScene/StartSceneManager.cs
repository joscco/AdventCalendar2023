using Code.GameScene.UI;
using DG.Tweening;
using General;
using General.OptionScreen;
using SceneManagement;
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
            optionScreenButton.OnButtonHover += () => optionScreenButton.Select();
            optionScreenButton.OnButtonExit += () => optionScreenButton.Deselect();
            optionScreenButton.OnButtonClick += ActivateOptionButton;
            startButton.OnButtonHover += () => startButton.Select();
            startButton.OnButtonExit += () => startButton.Deselect();
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