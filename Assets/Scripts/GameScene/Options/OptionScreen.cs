using DG.Tweening;
using UnityEngine;

namespace GameScene.Options
{
    public class OptionScreen : MonoBehaviour
    {
        [SerializeField] private int offsetDownStart = -900;
        [SerializeField] private LeftRightOptionGroup languageGroup;
        [SerializeField] private LeftRightOptionGroup musicVolumeGroup;
        [SerializeField] private LeftRightOptionGroup soundEffectVolumeGroup;
        [SerializeField] private GameObject selectionRect;

        private LeftRightOptionGroup _focusedGroup;

        public static OptionScreen instance;

        private bool _showing;

        private void Start()
        {
            instance = this;
            var position = transform.position;
            position.y = offsetDownStart;
            transform.position = position;

            ChangeFocusedGroup(languageGroup);

            languageGroup.wantsFocus += () => ChangeFocusedGroup(languageGroup);
            musicVolumeGroup.wantsFocus += () => ChangeFocusedGroup(musicVolumeGroup);
            soundEffectVolumeGroup.wantsFocus += () => ChangeFocusedGroup(soundEffectVolumeGroup);
        }

        public void BlendIn()
        {
            _showing = true;
            transform.DOMoveY(0, 0.5f).SetEase(Ease.InOutQuad);
        }

        public void BlendOut()
        {
            _showing = false;
            transform.DOMoveY(offsetDownStart, 0.5f).SetEase(Ease.InOutQuad);
        }

        public bool IsShowing()
        {
            return _showing;
        }

        public void HandleUpdate()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                _focusedGroup.DecreaseValue();
                return;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                _focusedGroup.IncreaseValue();
                return;
            }
            
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                DecreaseFocusedGroupIndex();
                return;
            }
            
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                IncreaseFocusedGroupIndex();
            }
        }

        private void IncreaseFocusedGroupIndex()
        {
            if (_focusedGroup == languageGroup)
            {
                ChangeFocusedGroup(musicVolumeGroup);
            } else if (_focusedGroup == musicVolumeGroup)
            {
                ChangeFocusedGroup(soundEffectVolumeGroup);
            }
        }

        private void DecreaseFocusedGroupIndex()
        {
            if (_focusedGroup == musicVolumeGroup)
            {
                ChangeFocusedGroup(languageGroup);
            } else if (_focusedGroup == soundEffectVolumeGroup)
            {
                ChangeFocusedGroup(musicVolumeGroup);
            }
        }

        private void ChangeFocusedGroup(LeftRightOptionGroup group)
        {
            if (group != _focusedGroup)
            {
                _focusedGroup = group;
                selectionRect.transform
                    .DOMove(group.transform.position, 0.2f)
                    .SetEase(Ease.OutBack);
            }
        }

        public void Toggle()
        {
            if (IsShowing())
            {
                BlendOut();
            }
            else
            {
                BlendIn();
            }
        }
    }
}