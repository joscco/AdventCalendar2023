using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameScene.Dialog
{
    public class DialogBubble : MonoBehaviour
    {
        [SerializeField] private float maxTextWidth = 450f;
        [SerializeField] private int paddingX = 30;
        [SerializeField] private int paddingY = 15;

        [SerializeField] private TextMeshPro textObject;
        [SerializeField] private SpriteRenderer bubbleRenderer;

        private Tween _typeseq;

        private void Start()
        {
            InstantResizeBubble("");
            textObject.text = "";
            textObject.maxVisibleCharacters = 0;
            transform.localScale = new Vector3(1, 0, 1);
        }

        public Sequence BlendOutBubble()
        {
            return DOTween.Sequence()
                .Append(ResizeBubble(""))
                .Append(RescaleAll(0));
        }

        public Sequence BlendInEmptyBubble()
        {
            return DOTween.Sequence()
                .Append(InstantResizeBubble(""))
                .Append(RescaleAll(1));
        }

        public Sequence Type(string newText)
        {
            return DOTween.Sequence()
                .Append(ResizeBubble(newText))
                .AppendCallback(() =>
                {
                    textObject.text = newText;
                })
                .Append(DoVisibleCharacters(textObject, newText.Length, newText.Length * 0.01f));
        }

        public Tween Detype()
        {
            return DoVisibleCharacters(textObject, 0, 0.2f);
        }

        private Sequence InstantResizeBubble(string newText)
        {
            var paddedSize = CalculatePaddedSize(newText);

            return DOTween.Sequence()
                .AppendCallback(() =>
                {
                    textObject.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, paddedSize.y);
                    bubbleRenderer.size = paddedSize;
                });
        }

        private Sequence ResizeBubble(string newText)
        {
            var paddedSize = CalculatePaddedSize(newText);

            return DOTween.Sequence()
                .AppendCallback(() =>
                {
                    textObject.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, paddedSize.y);
                })
                .Append(TweenResizeBubble(paddedSize, 0.3f));
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
            return transform.DOScaleY(scale, 0.2f)
                .SetEase(Ease.InOutQuad);
        }

        private Tween DoVisibleCharacters(TextMeshPro textMeshPro, int newLength, float duration)
        {
            //_typeseq?.Kill();
            _typeseq = DOTween.To(
                    () => textMeshPro.maxVisibleCharacters,
                    (len) => textMeshPro.maxVisibleCharacters = len,
                    newLength,
                    duration)
                .SetEase(Ease.Linear);
            return _typeseq;
        }

        private Tween TweenResizeBubble(Vector2 newSize, float duration)
        {
            return DOTween.To(
                    () => bubbleRenderer.size,
                    (size) => bubbleRenderer.size = size,
                    newSize,
                    duration)
                .SetEase(Ease.InOutQuad);
        }
    }
}