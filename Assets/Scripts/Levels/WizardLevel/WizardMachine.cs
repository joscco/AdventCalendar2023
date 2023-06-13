using DG.Tweening;
using GameScene.PlayerControl;
using UnityEngine;

namespace Levels.WizardLevel
{
    public class WizardMachine : MovableGridEntity
    {
        [SerializeField] private WizardMachineDirection _direction;
        [SerializeField] private GameObject mirrorLayer;

        public void Start()
        {
            UpdateMirrorLayer();
        }

        public void ToggleDirection()
        {
            if (_direction == WizardMachineDirection.Horizontal)
            {
                SetDirection(WizardMachineDirection.Vertical);
            }
            else
            {
                SetDirection(WizardMachineDirection.Horizontal);
            }
        }

        public void SetDirection(WizardMachineDirection newDirection)
        {
            _direction = newDirection;
            UpdateMirrorLayer();
        }

        private void UpdateMirrorLayer()
        {
            if (_direction == WizardMachineDirection.Horizontal)
            {
                mirrorLayer.transform.DORotate(new Vector3(0, 0, 0), 0.3f)
                    .SetEase(Ease.InOutQuad);
            }
            else
            {
                mirrorLayer.transform.DORotate(new Vector3(0, 0, 90), 0.3f)
                    .SetEase(Ease.InOutQuad);
            }
        }


        public enum WizardMachineDirection
        {
            Horizontal, Vertical
        }

        public WizardMachineDirection GetDirection()
        {
            return _direction;
        }
    }
}