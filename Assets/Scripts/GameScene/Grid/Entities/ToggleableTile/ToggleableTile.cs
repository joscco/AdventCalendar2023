using DG.Tweening;
using GameScene.Grid.Entities.Shared;
using General.Grid;
using UnityEngine;
using UnityEngine.Serialization;

namespace Levels.WizardLevel
{
    public class ToggleableTile : GridEntity
    {
        [SerializeField] private bool _active;
        [SerializeField] private SpriteRenderer _innerSpriteRenderer;

        public void Toggle()
        {
            SetActive(!_active);
        }

        public void SetActive(bool active)
        {
            _active = active;
            BlendInnerTile(active ? 1f : 0f);
        }

        private void BlendInnerTile(float value)
        {
            _innerSpriteRenderer.DOFade(value, 0.3f)
                .SetEase(Ease.InOutQuad);
        }

        public bool IsActive()
        {
            return _active;
        }
    }
}