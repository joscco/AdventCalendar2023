
using Code.GameScene.Inventory;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code
{
    public class Game : MonoBehaviour
    {
        [FormerlySerializedAs("inventoryMono")] [FormerlySerializedAs("inventory")] public InventoryInstance inventoryInstance;
        
    }
}