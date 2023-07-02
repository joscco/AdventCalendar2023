using DG.Tweening;
using UnityEngine;

namespace GameScene.Dialog
{
    public class DialogBox : MonoBehaviour
    {
        [SerializeField] private DialogBubble bubble;
        private Sequence animationSequence;
        private bool showingBubble;
        private bool showingText;

        private void Start()
        {
            DOVirtual.DelayedCall(2, () => Show("Hello You!"));
            DOVirtual.DelayedCall(5, () => Show("How are you doing? I hope you're doing great! llfa;sdfk;akhaakjsdhfalksjdhfaljkhfkashdf"));
            DOVirtual.DelayedCall(8, () => Show("I'm doing great!"));
            DOVirtual.DelayedCall(11, () => Hide());
        }

        public Sequence Show(string newText, float delay = 0f)
        {
            var seq = DOTween.Sequence()
                .AppendInterval(delay);
            
            if (!showingBubble)
            {
                seq.Append(bubble.BlendInEmptyBubble());
                showingBubble = true;
            }
        
            if (showingText)
            {
                seq.Append(bubble.Detype());
                showingText = false;
            }

            seq.Append(bubble.Type(newText));
            showingText = true;

            return seq;
        }

        public Sequence Hide(float delay = 0f)
        {
            var seq = DOTween.Sequence()
                .AppendInterval(delay);
            
            if (showingText)
            {
                seq.Append(bubble.Detype());
                showingText = false;
            }

            if (showingBubble)
            {
                seq.Append(bubble.BlendOutBubble());
                showingBubble = false;
            }
            return seq;
        }
    }
}