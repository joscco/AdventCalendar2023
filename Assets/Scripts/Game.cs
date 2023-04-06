
using System;
using Code.GameScene.Inventory;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code
{
    public class Game : MonoBehaviour
    {
        public static Game Instance = null;
        
        public PlayerData PlayerData;
        
        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
        
        private void Start()
        {
            PlayerData = new PlayerData();
        }
    }
}