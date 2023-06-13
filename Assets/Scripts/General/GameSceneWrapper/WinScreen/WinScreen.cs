using DG.Tweening;
using UnityEngine;

namespace General.WinScreen
{
    public class WinScreen : MonoBehaviour
    {
        [SerializeField] private int offsetDownStart = -900;
        [SerializeField] private TitleAnimation titleAnimation;

        private void Start()
        {
            Vector3 pos = transform.position;
            pos.y = offsetDownStart;
            transform.position = pos;
        }

        public void BlendIn(float delay)
        {
            Debug.Log("Blend in Win Screen");
            transform.DOMoveY(0, 0.5f)
                .SetDelay(delay)
                .SetEase(Ease.InOutQuad);
            titleAnimation.FadeIn();
        }
    
        public void BlendOut()
        {
            transform.DOMoveY(offsetDownStart, 0.5f).SetEase(Ease.InOutQuad);
        }

        public void HandleUpdate()
        {
            
        }
    }
}
