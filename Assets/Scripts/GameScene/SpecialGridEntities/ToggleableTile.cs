using GameScene.PlayerControl;
using General.Grid;
using UnityEngine;

namespace Levels.WizardLevel
{
    public class ToggleableTile : GridEntity
    {
        [SerializeField] private bool _active;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite _activeSprite;
        [SerializeField] private Sprite _inactiveSprite;

        public void Toggle()
        {
            SetActive(!_active);
        }

        public void SetActive(bool val)
        {
            _active = val;
            _spriteRenderer.sprite = val ? _activeSprite : _inactiveSprite;
        }

        public bool IsActive()
        {
            return _active;
        }

        public void SetSortOrder(int order)
        {
            spriteRenderers.ForEach(rend => rend.sortingOrder = order);
        }
    }
}