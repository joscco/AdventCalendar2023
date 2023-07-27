using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace GameScene.Dialog.Bubble
{
    public class DialogBubbleRenderer : MonoBehaviour
    {
        [SerializeField] private float maxTextWidth = 450f;
        [SerializeField] private int paddingX = 30;
        [SerializeField] private int paddingY = 15;

        [SerializeField] private TextMeshPro textObject;
        [SerializeField] private SpriteRenderer bubbleRenderer;
        [SerializeField] private DialogBubbleNextHint nextHint;

        private Sequence _bubbleSeq;
        private Tween _typeSeq;
        private Tween _typingSeq;
        private Tween _bubbleSizeTween;
        private Tween _bubbleScaleTween;

        private void Start()
        {
            InstantResizeBubble("");
            textObject.text = "";
            textObject.maxVisibleCharacters = 0;
            transform.localScale = new Vector3(0, 0, 1);
        }

        public Sequence BlendOutBubble()
        {
            _bubbleSeq?.Kill();
            _bubbleSeq = DOTween.Sequence()
                .Append(ResizeBubble(""))
                .Append(RescaleAll(0));
            return _bubbleSeq;
        }

        public Sequence RescaleBubbleToEmptyText()
        {
            _bubbleSeq?.Kill();
            _bubbleSeq = DOTween.Sequence()
                .Append(ResizeBubble(""))
                .Append(RescaleAll(1));
            return _bubbleSeq;
        }

        public Tween Type(string newText)
        {
            _typeSeq?.Kill();
            _typeSeq = DOTween.Sequence()
                .Append(ResizeBubble(newText))
                .AppendCallback(() => { textObject.text = newText; })
                .Join(DoVisibleCharacters(textObject, newText.Length, newText.Length * 0.01f));
            return _typeSeq;
        }

        public Tween Detype()
        {
            return DoVisibleCharacters(textObject, 0, 0.1f);
        }

        private void InstantResizeBubble(string newText)
        {
            var paddedSize = CalculatePaddedSize(newText);
            textObject.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, paddedSize.y);
            bubbleRenderer.size = paddedSize;
        }

        private Sequence ResizeBubble(string newText)
        {
            var paddedSize = CalculatePaddedSize(newText);

            return DOTween.Sequence()
                .AppendCallback(() =>
                {
                    textObject.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, paddedSize.y);
                })
                .Append(TweenResizeBubble(paddedSize, 0.15f))
                .Join(nextHint.transform.DOLocalMove(new Vector2(paddedSize.x / 2, paddedSize.y), 0.15f));
        }

        private Vector2 CalculatePaddedSize(string newText)
        {
            var rawPreferredSize = textObject.GetPreferredValues(newText, maxTextWidth, float.PositiveInfinity);
            var preferredHeight = rawPreferredSize.y;
            var preferredWidth = Math.Clamp(rawPreferredSize.x, 0f, maxTextWidth);
            return new Vector2(preferredWidth + 2 * paddingX, preferredHeight + 2 * paddingY);
        }

        private Tween RescaleAll(float scale)
        {
            _bubbleScaleTween?.Kill();
            _bubbleScaleTween = transform.DOScale(scale, 0.2f)
                .SetEase(Ease.InOutQuad);
            return _bubbleScaleTween;
        }

        private Tween DoVisibleCharacters(TextMeshPro textMeshPro, int newLength, float duration)
        {
            _typingSeq?.Kill();
            _typingSeq = DOTween.To(
                    () => textMeshPro.maxVisibleCharacters,
                    (len) => textMeshPro.maxVisibleCharacters = len,
                    newLength,
                    duration)
                .SetEase(Ease.Linear);
            return _typingSeq;
        }

        private Tween TweenResizeBubble(Vector2 newSize, float duration)
        {
            _bubbleSizeTween?.Kill();
            _bubbleSizeTween = DOTween.To(
                    () => bubbleRenderer.size,
                    (size) => bubbleRenderer.size = size,
                    newSize,
                    duration)
                .SetEase(Ease.InOutQuad);
            return _bubbleSizeTween;
        }

        public Tween BlendOutContinuationHint()
        {
            return nextHint.BlendOut();
        }

        public Tween BlendInKeyHintToContinue()
        {
            return nextHint.BlendIn();
        }
    }
}