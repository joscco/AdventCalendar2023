using System.Collections.Generic;
using GameScene.Grid.Entities.Shared;
using General.Grid;
using UnityEngine;

namespace Levels.WizardLevel
{
    public class ToggleableTileSwitch : GridEntity
    {
        [SerializeField] private bool _active;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite _activeSprite;
        [SerializeField] private Sprite _inactiveSprite;
        [SerializeField] private List<ToggleableTile> _tilesToCommand;

        public void Toggle()
        {
            SetActive(!_active);
        }

        public void SetActive(bool val)
        {
            _active = val;
            _spriteRenderer.sprite = val ? _activeSprite : _inactiveSprite;
            _tilesToCommand.ForEach(tile => tile.Toggle());
        }

        public bool IsActive()
        {
            return _active;
        }
    }
}