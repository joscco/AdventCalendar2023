using UnityEngine;

namespace SceneManagement
{
    public class GameSceneWrapper : MonoBehaviour
    {

        private static GameSceneWrapper _instance;
        
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this);
            }
            else
            {
                _instance = this;
            }
        }

        public static GameSceneWrapper Get()
        {
            return _instance;
        }
    }
}