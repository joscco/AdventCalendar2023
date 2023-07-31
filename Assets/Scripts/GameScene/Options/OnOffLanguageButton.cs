using DG.Tweening;
using TMPro;
using UI;
using UnityEngine;

namespace GameScene.Options
{
    public class OnOffLanguageButton : OnOffSpriteButton
    {
        [SerializeField] private Transform checkMarkTransform;
        [SerializeField] private TextMeshPro text;
        
        public override void SetOn()
        {
            SetActiveSprite();
            checkMarkTransform.DOScale(1, 0.2f).SetEase(Ease.InOutQuad);
            text.DOFade(1, 0.2f).SetEase(Ease.InOutQuad);
        }

        public override void SetOff()
        {
            SetInactiveSprite();
            checkMarkTransform.DOScale(0, 0.2f).SetEase(Ease.InOutQuad);
            text.DOFade(0.5f, 0.2f).SetEase(Ease.InOutQuad);
        }
    }
}