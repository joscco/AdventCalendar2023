using GameScene.Inventory;
using GameScene.Items.Item;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameScene
{
    public class Level : MonoBehaviour
    {
        private static Level _instance;

        public LevelData levelData;
        public SampleList sampleTypes;
        public ItemWiki itemWiki;
        public WinScreen.WinScreen winScreen;

        public static Level Get()
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