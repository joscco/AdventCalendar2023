using UnityEngine;
using UnityEngine.EventSystems;

namespace GameScene.Items.Scripts.Field
{
    public abstract class FieldSpot : MonoBehaviour, IPointerUpHandler
    {
        public int row;
        public int column;
        public FieldEntity fieldEntity;

        public FieldEntity CreateNewEntity(InventoryItem item)
        {
            var newEntityPrefab = Game.Instance.InventoryItemWiki.GetPrefabForItem(item);
            return Instantiate(newEntityPrefab, transform);
        }

        public void Clear()
        {
            fieldEntity = null;
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            // if (GameManager.Instance.ModeManager.Mode == GameMode.Planting
            //     && fieldEntity == null
            //     && GameManager.Instance.Inventory.selectedItem != InventoryItem.None)
            // {
            //     var selectedItem = GameManager.Instance.Inventory.selectedItem;
            //     if (GameManager.Instance.Inventory.GetItemCount(selectedItem) > 0)
            //     {
            //         GameManager.Instance.Inventory.RemoveInventoryItem(selectedItem);
            //         fieldEntity = CreateNewEntity(selectedItem);
            //         fieldEntity.BlendIn();
            //         fieldEntity.StartEvolution();
            //     }
            // }
        }
    }
}