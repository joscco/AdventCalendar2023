using DG.Tweening;
using UnityEngine;

namespace General.LoseScreen
{
    public class LoseScreen : MonoBehaviour
    {
        [SerializeField] private int offsetDownStart = -900;

        private void Start()
        {
            Vector3 pos = transform.position;
            pos.y = offsetDownStart;
            transform.position = pos;
        }

        public void BlendIn(float delay)
        {
            transform.DOMoveY(0, 0.5f)
                .SetDelay(delay)
                .SetEase(Ease.InOutQuad);
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
