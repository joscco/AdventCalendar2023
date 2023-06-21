using General.Grid;
using UnityEngine;

namespace Levels.WizardLevel
{
    public class PickableItemHolder: MonoBehaviour
    {
        [SerializeField] private PickableItem pickItem;

        public bool HasItem()
        {
            return pickItem != null;
        }

        public PickableItem GetItem()
        {
            return pickItem;
        }

        public void SetItem(PickableItem item)
        {
            pickItem = item;
        }
        
        public void RemoveItem()
        {
            SetItem(null);
        }

        public Transform GetPickupPoint()
        {
            return transform;
        }
    }
}