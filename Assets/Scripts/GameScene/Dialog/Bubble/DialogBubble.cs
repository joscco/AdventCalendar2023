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

        public Sequence Show(string newText, bool showHint = true, float delay = 0f)
        {
            _bubbleSequence?.Kill();
            _bubbleSequence = DOTween.Sequence()
                .AppendInterval(delay);

            if (!_showingBubble)
            {
                _bubbleSequence.Append(bubbleRenderer.BlendInEmptyBubble())
                    .OnStart(() => _showingBubble = true);
            }

            _bubbleSequence.Append(bubbleRenderer.Detype());
            _bubbleSequence.Append(bubbleRenderer.Type(newText));
            _bubbleSequence.Join(showHint ? bubbleRenderer.BlendInHint() : bubbleRenderer.BlendOutHint());

            return _bubbleSequence;
        }

        public Sequence ShowDotHint()
        {
            return Show("...", false);
        }

        public Sequence Hide(float delay = 0f)
        {
            _bubbleSequence?.Kill();
            _bubbleSequence = DOTween.Sequence()
                .AppendInterval(delay);


            _bubbleSequence.Append(bubbleRenderer.BlendOutHint());
            _bubbleSequence.Join(bubbleRenderer.Detype());

            if (_showingBubble)
            {
                _bubbleSequence.Append(bubbleRenderer.BlendOutBubble())
                    .OnComplete(() => _showingBubble = false);
            }

            return _bubbleSequence;
        }
    }
}