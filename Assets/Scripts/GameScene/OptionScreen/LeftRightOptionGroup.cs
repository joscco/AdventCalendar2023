using System.Collections.Generic;
using Code.GameScene.UI;
using DG.Tweening;
using UnityEngine;

namespace General.OptionScreen
{
    public class LeftRightOptionGroup : MonoBehaviour
    {
        [SerializeField] private List<ScalingButton> optionsFromLeftToRight;
        [SerializeField] private int startIndex;
        

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
            // To Implement
        }

        public void Toggle()
        {
            if (IsShowing())
            {
                BlendOut();
            }
            else
            {
                BlendIn();
            }
        }
    }
}
