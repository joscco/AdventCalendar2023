using DG.Tweening;
using General;
using SceneManagement;
using UI;
using UnityEngine;

namespace GameScene.WinScreen
{
    public class WinScreen : MonoBehaviour
    {
        [SerializeField] private int offsetDownStart = -900;
        [SerializeField] private TitleAnimation titleAnimation;
        
        [SerializeField] private ScalingButton levelScreenButton;
        [SerializeField] private ScalingButton nextButton;
        [SerializeField] private GameObject selectionRectangle;
        
        private ScalingButton _selectedButton;

        private void Start()
        {
            Hide();

            levelScreenButton.OnButtonHover += () => ChangeButtonSelection(levelScreenButton);
            levelScreenButton.OnButtonClick += ActivateLevelScreenButton;
            nextButton.OnButtonHover += () => ChangeButtonSelection(nextButton);
            nextButton.OnButtonClick += ActivateNextButton;
        }

        private void ActivateNextButton()
        {
            SceneTransitionManager.Get().TransitionToNextLevel();
            nextButton.ScaleUpThenDown();
        }

        private void ActivateLevelScreenButton()
        {
            SceneTransitionManager.Get().TransitionToNonLevelScene("LevelChoosingScene");
            levelScreenButton.ScaleUpThenDown();
        }

        private void Hide()
        {
            Vector3 pos = transform.position;
            pos.y = offsetDownStart;
            transform.position = pos;
        }

        public void BlendIn(float delay)
        {
            DOTween.Sequence()
                .AppendInterval(delay)
                .Append(transform.DOMoveY(0, 0.5f).SetEase(Ease.InOutQuad))
                .Append(titleAnimation.FadeIn());
            ChangeButtonSelection(nextButton, true);
        }

        private void ChangeButtonSelection(ScalingButton newButton, bool instant = false)
        {
            if (_selectedButton)
            {
                _selectedButton.ScaleDown();
            }

            newButton.ScaleUp();
            _selectedButton = newButton;
            if (instant)
            {
                selectionRectangle.transform.position = newButton.transform.position;
            }
            else
            {
                selectionRectangle.transform.DOMove(newButton.transform.position, 0.2f)
                    .SetEase(Ease.OutBack);
            }
        }

        public void HandleUpdate()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                if (_selectedButton != levelScreenButton)
                {
                    ChangeButtonSelection(levelScreenButton);
                }

                return;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                if (_selectedButton != nextButton)
                {
                    ChangeButtonSelection(nextButton);
                }
            }
            
            if (Input.GetKeyDown(KeyCode.Space) && null != _selectedButton)
            {
                if (_selectedButton == nextButton)
                {
                    ActivateNextButton();
                }
                else
                {
                    ActivateLevelScreenButton();
                }
            }
        }
    }
}