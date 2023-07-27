using DG.Tweening;
using GameScene.Dialog.Background;
using UnityEngine;

namespace GameScene.Dialog.Bubble
{
    public class DialogBubble : MonoBehaviour
    {
        [SerializeField] private DialogBubbleRenderer bubbleRenderer;

        public DialogSpeaker speaker;

        private bool _showingBubble;
        private Sequence _bubbleSequence;

        public Sequence ShowText(string newText, bool showContinueHint = true, float delay = 0f)
        {
            _bubbleSequence?.Kill();
            _bubbleSequence = DOTween.Sequence()
                .AppendInterval(delay);

            _bubbleSequence.Append(bubbleRenderer.Detype());
            _bubbleSequence.Append(bubbleRenderer.RescaleBubbleToEmptyText());
            _bubbleSequence.Append(bubbleRenderer.Type(newText));
            _bubbleSequence.Join(showContinueHint
                ? bubbleRenderer.BlendInKeyHintToContinue()
                : bubbleRenderer.BlendOutContinuationHint());

            return _bubbleSequence;
        }

        public Sequence ShowDotHint()
        {
            return ShowText("...", false);
        }

        public Sequence Hide(float delay = 0f)
        {
            _bubbleSequence?.Kill();
            _bubbleSequence = DOTween.Sequence()
                .AppendInterval(delay);

            _bubbleSequence.Append(bubbleRenderer.BlendOutContinuationHint());
            _bubbleSequence.Join(bubbleRenderer.Detype());
            _bubbleSequence.Append(bubbleRenderer.BlendOutBubble());

            return _bubbleSequence;
        }
    }
}