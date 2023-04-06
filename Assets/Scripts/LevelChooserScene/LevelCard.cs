using System;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.LevelChooserScene
{
    public class LevelCard : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private TextMeshPro levelTitle;
        [SerializeField] private TextMeshPro levelNumber;

        private LevelCardData _data;
        private bool _shown;

        private void Awake()
        {
            transform.localScale = Vector3.zero;
            _shown = false;
        }

        public void SetLevelCardData(LevelCardData newData)
        {
            _data = newData;
            UpdateCardRenderer();
        }

        public void BlendIn()
        {
            transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
            _shown = true;
        }

        public void BlendOut()
        {
            transform.DOScale(0f, 0.3f).SetEase(Ease.InBack);
            _shown = false;
        }

        private void UpdateCardRenderer()
        {
            if (_data)
            {
                spriteRenderer.sprite = _data.sprite;
                levelNumber.text = _data.level.ToString();
                levelTitle.text = _data.levelName;
            }
        }

        private void OnMouseEnter()
        {
            if (_shown && !SceneTransitionManager.Get().IsInTransition())
            {
                transform.DOScale(1.1f, 0.3f).SetEase(Ease.OutBack);
            }
        }

        private void OnMouseExit()
        {
            if (_shown && !SceneTransitionManager.Get().IsInTransition())
            {
                transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
            }
        }

        private void OnMouseUp()
        {
            if (_shown && !SceneTransitionManager.Get().IsInTransition())
            {
                SceneTransitionManager.Get().TransitionTo("GameScene");
            }
        }

        public int GetLevelNumber()
        {
            return _data ? _data.level : -1;
        }
    }
}