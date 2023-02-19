using Unity.VisualScripting;
using UnityEngine;

namespace GameScene
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public Inventory Inventory { get; private set; }
        public ModeManager ModeManager { get; private set; }
        public InventoryItemWiki InventoryItemWiki { get; private set; }
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            Inventory = GetComponentInChildren<Inventory>();
            ModeManager = GetComponentInChildren<ModeManager>();
            InventoryItemWiki = GetComponentInChildren<InventoryItemWiki>();
        }
    }
}