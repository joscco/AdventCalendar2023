using System;
using DG.Tweening;
using General;
using UI;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace GameScene.Options
{
    public class OptionScreen : MonoBehaviour
    {
        [SerializeField] private int offsetDownStart = -900;
        [SerializeField] private ScalingButton cancelButton;
        [SerializeField] private LeftRightOptionGroup languageGroup;
        [SerializeField] private Locale englishLocale;
        [SerializeField] private Locale germanLocale;
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

            cancelButton.OnButtonHover += () => cancelButton.ScaleUp();
            cancelButton.OnButtonExit += () => cancelButton.ScaleDown();
            cancelButton.OnButtonClick += () => Toggle();

            ChangeFocusedGroup(languageGroup);

            languageGroup.wantsFocus += () => ChangeFocusedGroup(languageGroup);
            musicVolumeGroup.wantsFocus += () => ChangeFocusedGroup(musicVolumeGroup);
            soundEffectVolumeGroup.wantsFocus += () => ChangeFocusedGroup(soundEffectVolumeGroup);

            languageGroup.changeValue += (value) =>
            {
                LocalizationSettings.SelectedLocale = value == 0 ? englishLocale : germanLocale;
                Game.instance.SavePreferredLanguage(value == 0 ? Game.LanguageIdentifier.En : Game.LanguageIdentifier.De);
            };
            musicVolumeGroup.changeValue += (value) => AudioManager.instance.SetMusicVolume(value * 1f / 9);
            soundEffectVolumeGroup.changeValue += (value) => AudioManager.instance.SetSFXVolume(value * 1f / 9);

            var startMusicVol = Mathf.RoundToInt(Game.instance.GetMusicVolume() * 9);
            var startSfxVol = Mathf.RoundToInt(Game.GetSfxVolume() * 9);
            var startLanguage = Game.instance.GetPreferredLanguage() == Game.LanguageIdentifier.En ? 0 : 1;

            musicVolumeGroup.SetValue(startMusicVol);
            soundEffectVolumeGroup.SetValue(startSfxVol);
            languageGroup.SetValue(startLanguage);
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
            }
            else if (_focusedGroup == musicVolumeGroup)
            {
                ChangeFocusedGroup(soundEffectVolumeGroup);
            }
        }

        private void DecreaseFocusedGroupIndex()
        {
            if (_focusedGroup == musicVolumeGroup)
            {
                ChangeFocusedGroup(languageGroup);
            }
            else if (_focusedGroup == soundEffectVolumeGroup)
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
                cancelButton.ScaleUpThenDown();
                BlendOut();
            }
            else
            {
                BlendIn();
            }
        }
    }
}