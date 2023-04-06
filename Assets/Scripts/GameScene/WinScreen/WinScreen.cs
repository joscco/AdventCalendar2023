using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace GameScene.WinScreen
{
    public class WinScreen : MonoBehaviour
    {
        [SerializeField] private int offsetDownStart = -900;
        [SerializeField] private ParticleSystem particleSystem;
    
        private void Start()
        {
            particleSystem.Clear();
            particleSystem.Stop();
            Vector3 pos = transform.position;
            pos.y = offsetDownStart;
            transform.position = pos;
        }

        public void BlendIn()
        {
            particleSystem.Play();
            transform.DOMoveY(0, 0.5f).SetEase(Ease.InOutQuad);
        }
    
        public void BlendOut()
        {
            particleSystem.Stop();
            transform.DOMoveY(offsetDownStart, 0.5f).SetEase(Ease.InOutQuad);
        }
    }
}
