using GameScene.Options;
using UI;
using UnityEngine;

namespace GameScene.OptionScreen
{
    public class OnOffSpriteButton : OnOffButton
    {
        [SerializeField] private Sprite spriteWhenActive;
        [SerializeField] private Sprite spriteWhenInactive;
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        public override void SetOn()
        {
            spriteRenderer.sprite = spriteWhenActive;
        }

        public override void SetOff()
        {
            spriteRenderer.sprite = spriteWhenInactive;
        }
    }
}