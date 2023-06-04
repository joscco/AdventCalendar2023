using DG.Tweening;
using UnityEngine;

namespace General.OptionScreen
{
    public class OptionScreen : MonoBehaviour
    {
        [SerializeField] private int offsetDownStart = -900;

        private bool showing;

        private void Start()
        {
            Vector3 pos = transform.position;
            pos.y = offsetDownStart;
            transform.position = pos;
        }

        public void BlendIn()
        {
            showing = true;
            transform.DOMoveY(0, 0.5f).SetEase(Ease.InOutQuad);
        }
    
        public void BlendOut()
        {
            showing = false;
            transform.DOMoveY(offsetDownStart, 0.5f).SetEase(Ease.InOutQuad);
        }

        public bool IsShowing()
        {
            return showing;
        }

        public void HandleUpdate()
        {
            throw new System.NotImplementedException();
        }
    }
}
