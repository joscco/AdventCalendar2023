using DG.Tweening;
using GameScene.PlayerControl;
using UnityEngine;

namespace Levels.WizardLevel
{
    public class WizardMachine : MovableGridEntity
    {
        [SerializeField] private WizardMachineDirection _direction;
        [SerializeField] private WizardMachineLaser leftLaser;
        [SerializeField] private WizardMachineLaser rightLaser;
        [SerializeField] private WizardMachineLaser topLaser;
        [SerializeField] private WizardMachineLaser downLaser;

        public void Start()
        {
            leftLaser.BlendOutInstantly();
            rightLaser.BlendOutInstantly();
            topLaser.BlendOutInstantly();
            downLaser.BlendOutInstantly();
            UpdateLasers();
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
            UpdateLasers();
        }

        private void UpdateLasers()
        {
            if (_direction == WizardMachineDirection.Horizontal)
            {
                leftLaser.BlendIn();
                rightLaser.BlendIn();
                topLaser.BlendOut();
                downLaser.BlendOut();
            }
            else
            {
                leftLaser.BlendOut();
                rightLaser.BlendOut();
                topLaser.BlendIn();
                downLaser.BlendIn();
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