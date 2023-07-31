using GameScene.Options;
using UnityEngine;

namespace UI
{
    public class OnOffSpriteButton : OnOffButton
    {
        [SerializeField] private Sprite spriteWhenActive;
        [SerializeField] private Sprite spriteWhenInactive;
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        public override void SetOn()
        {
            SetActiveSprite();
        }

        public override void SetOff()
        {
            SetInactiveSprite();
        }

        protected void SetActiveSprite()
        {
            spriteRenderer.sprite = spriteWhenActive;
        }
        
        protected void SetInactiveSprite()
        {
            spriteRenderer.sprite = spriteWhenInactive;
        }
    }
}