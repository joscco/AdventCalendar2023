using GameScene.PlayerControl;
using UnityEngine;

namespace Levels.WizardLevel
{
    public class WizardYellowTile : MovableGridEntity
    {
        private bool _active;
        private Vector2Int _storedIndex;

        public void SetActive(bool val)
        {
            _active = val;
        }
        
        public bool IsActive()
        {
            return _active;
        }

        public void SetSortOrder(int order)
        {
            spriteRenderer.sortingOrder = order;
        }

        public void StoreIndex(Vector2Int index)
        {
            _storedIndex = index;
        }

        public Vector2Int GetStoredIndex()
        {
            return _storedIndex;
        }
    }
}