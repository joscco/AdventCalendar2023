using System.Collections.Generic;
using UnityEngine;

namespace SceneManagement
{
    public class GridAdapter: MonoBehaviour
    {
        [SerializeField] private Grid grid;
        
        public Vector2 GetPositionForIndex(Vector2Int index)
        {
            return grid.GetCellCenterWorld(IndexWrap(index));
        }
        
        public Vector3Int IndexWrap(Vector2Int index)
        {
            return new Vector3Int(index.x, index.y, 0);
        }
    }
}