using System.Collections.Generic;
using DG.Tweening;
using GameScene.Grid.Entities.Shared;
using UnityEngine;

namespace Levels.WizardLevel
{
    public class ToggleableTileSwitch : GridEntity
    {
        [SerializeField] private bool _active;
        [SerializeField] private SpriteRenderer _toggleRenderer;
        [SerializeField] private float degreesOn = -45f;
        [SerializeField] private float degreesOff = 45f;
        [SerializeField] private List<ToggleableTile> _tilesToCommand;

        private void Start()
        {
            SetActive(_active);
        }

        public void Toggle()
        {
            SetActive(!_active);
        }

        public void SetActive(bool val)
        {
            _active = val;
            _toggleRenderer.transform
                .DORotate(new Vector3(0, 0 , val ? degreesOn : degreesOff) , 0.3f)
                .SetEase(Ease.InOutQuad);
            _tilesToCommand.ForEach(tile => tile.Toggle());
        }

        public bool IsActive()
        {
            return _active;
        }
    }
}