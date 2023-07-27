using UnityEngine;
using UnityEngine.Serialization;

namespace GameScene.Characters
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class BlinkingEye : MonoBehaviour
    {
        [SerializeField] private Sprite openEye;
        [SerializeField] private Sprite mediumEye;
        [SerializeField] private Sprite closedEye;

        [SerializeField] private SpriteRenderer spriteRenderer;

        public void ToMedium()
        {
            spriteRenderer.sprite = mediumEye;
        }

        public void ToClose()
        {
            spriteRenderer.sprite = closedEye;
        }

        public void ToOpen()
        {
            spriteRenderer.sprite = openEye;
        }
    }
}