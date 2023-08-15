using DG.Tweening;
using GameScene.Dialog.Background;
using GameScene.Dialog.Data;
using UnityEngine;
using UnityEngine.Localization;

namespace GameScene.Dialog.Bubble
{
    public class DialogBubble : MonoBehaviour
    {
        [SerializeField] private DialogBubbleRenderer bubbleRenderer;
        [SerializeField] private LocalizedString dotHintString;

        public DialogSpeaker speaker;

        private bool _showingBubble;
        private bool _showingHint;
        private LocalizedString _currentText;
        private Sequence _bubbleSequence;


        public Sequence ShowText(LocalizedString text, bool showContinueHint = true)
        {
            _currentText = text;
            _showingHint = showContinueHint;
            _bubbleSequence?.Kill();
            _bubbleSequence = DOTween.Sequence();
            _bubbleSequence.Append(bubbleRenderer.Detype());
            _bubbleSequence.Append(bubbleRenderer.RescaleBubbleToEmptyText());
            _bubbleSequence.Append(bubbleRenderer.Type(text.GetLocalizedString()));
            _bubbleSequence.Join(showContinueHint
                ? bubbleRenderer.BlendInKeyHintToContinue()
                : bubbleRenderer.BlendOutContinuationHint());

            return _bubbleSequence;
        }

        public Sequence ShowDotHint()
        {
            return ShowText(dotHintString, false);
        }

        public Sequence Hide()
        {
            _currentText = null;
            _bubbleSequence?.Kill();
            _bubbleSequence = DOTween.Sequence();

            _bubbleSequence.Append(bubbleRenderer.BlendOutContinuationHint());
            _bubbleSequence.Join(bubbleRenderer.Detype());
            _bubbleSequence.Append(bubbleRenderer.BlendOutBubble());

            return _bubbleSequence;
        }

        public void UpdateText()
        {
            if (_currentText != null)
            {
                ShowText(_currentText, _showingHint);
            }
        }
    }
}