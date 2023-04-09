using UnityEngine;

namespace GameScene
{
    public class LevelInputManager : MonoBehaviour
    {
        private bool _isTouch;

        public void Update()
        {
            if (!_isTouch)
            {
                if (Input.touches.Length > 0)
                {
                    _isTouch = true;
                }
            }
        }

        public bool IsTouch()
        {
            return _isTouch;
        }
    }
}