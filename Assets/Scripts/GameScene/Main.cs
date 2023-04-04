using Code.GameScene.Inventory;
using UnityEngine;

namespace Code.GameScene
{
    public class Main : MonoBehaviour
    {
        private static Main _instance;

        public LevelManager levelManager;
        public InventoryInstance inventory;
        public PlantWiki plantWiki;

        public static Main Get()
        {
            return _instance;
        }

        private void Start()
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
    }
}