using UnityEngine;

namespace General
{
    [RequireComponent(typeof(MeshRenderer))]
    public class ScrollingBackground : MonoBehaviour
    {
        [SerializeField] private float scrollSpeedX = -0.5f;
        [SerializeField] private float scrollSpeedY = 0.5f;
        [SerializeField] private Material backgroundMaterial;
    
        private MeshRenderer _spriteRenderer;
        private Vector2 _savedOffset;
    
        private void Start()
        {
            _savedOffset = backgroundMaterial.mainTextureOffset;
            _spriteRenderer = GetComponent<MeshRenderer>();
            _spriteRenderer.material = backgroundMaterial;
        }

        private void Update()
        {
            Vector2 newOffset = backgroundMaterial.mainTextureOffset + Time.deltaTime * new Vector2(scrollSpeedX, scrollSpeedY);
            backgroundMaterial.mainTextureOffset = new Vector2(newOffset.x % 1, newOffset.y % 1);
        }

        private void OnDisable()
        {
            backgroundMaterial.mainTextureOffset = _savedOffset;
        }
    }
}