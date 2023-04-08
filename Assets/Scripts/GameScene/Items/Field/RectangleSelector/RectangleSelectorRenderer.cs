using System;
using DG.Tweening;
using UnityEngine;

namespace GameScene.Items.Field.RectangleSelector
{
    public class RectangleSelectorRenderer : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite inactiveSelectionSprite;
        [SerializeField] private Sprite activeGoodSelectionSprite;
        [SerializeField] private Sprite activeBadSelectionSprite;
        
        private Vector2 _currentBottomLeft;
        private Vector2 _currentTopRight;

        private enum RectangleSelectorState
        {
            Hidden,
            ShowingInactive,
            ShowingActiveEnabled,
            ShowingActiveDisabled
        }

        private RectangleSelectorState _state;

        private void Start()
        {
            _state = RectangleSelectorState.Hidden;
            spriteRenderer.size = Vector2.zero;
            spriteRenderer.color = new Color(1, 1, 1, 0);
        }

        public void BlendOut()
        {
            spriteRenderer.DOFade(0, 0.2f).SetEase(Ease.InOutQuad);
            _state = RectangleSelectorState.Hidden;
        }
        
        private void BlendIn()
        {
            spriteRenderer.DOFade(0.5f, 0.2f).SetEase(Ease.InOutQuad);
        }

        public void ShowInactiveAt(Vector2 bottomLeft, Vector2 topRight)
        {
            if (_state != RectangleSelectorState.ShowingInactive)
            {
                spriteRenderer.sprite = inactiveSelectionSprite;
            }
            
            UpdateBox(bottomLeft, topRight);

            _state = RectangleSelectorState.ShowingInactive;
        }

        public void ShowActiveEnabledAt(Vector2 bottomLeft, Vector2 topRight)
        {
            if (_state != RectangleSelectorState.ShowingActiveEnabled)
            {
                spriteRenderer.sprite = activeGoodSelectionSprite;
            }
            
            UpdateBox(bottomLeft, topRight);

            _state = RectangleSelectorState.ShowingActiveEnabled;
        }

        public void ShowActiveDisabledAt(Vector2 bottomLeft, Vector2 topRight)
        {
            if (_state != RectangleSelectorState.ShowingActiveDisabled)
            {
                spriteRenderer.sprite = activeBadSelectionSprite;
            }

            UpdateBox(bottomLeft, topRight);

            _state = RectangleSelectorState.ShowingActiveDisabled;
        }

        private void UpdateBox(Vector2 bottomLeft, Vector2 topRight)
        {
            if (_state == RectangleSelectorState.Hidden)
            {
                BlendIn();
            }


            if (bottomLeft != _currentBottomLeft || topRight != _currentTopRight)
            {
                spriteRenderer.transform.DOMove(bottomLeft, 0.1f).SetEase(Ease.InOutQuad);
                DoSize(topRight - bottomLeft, 0.1f);
                
                _currentBottomLeft = bottomLeft;
                _currentTopRight = topRight;
            }
        }

        private void DoSize(Vector2 newSize, float duration)
        {
            DOTween.To(() => spriteRenderer.size,
                    (val) => { spriteRenderer.size = val; },
                    newSize,
                    duration)
                .SetEase(Ease.InOutQuad);
        }
    }
}