using System.Collections.Generic;
using UnityEngine;

namespace SceneManagement
{
    public class GridAdapter : MonoBehaviour
    {
        [SerializeField] private Grid grid;

        public Vector3 GetPositionForIndex(Vector2Int index)
        {
            var vec2 = grid.GetCellCenterWorld(IndexWrap(index));
            return new Vector3(vec2.x, vec2.y, index.y);
        }

        public Vector3Int IndexWrap(Vector2Int index)
        {
            return new Vector3Int(index.x, index.y, 0);
        }

        public Vector2Int IndexUnwrap(Vector3Int index)
        {
            return new Vector2Int(index.x, index.y);
        }

        public Vector3 PositionWrap(Vector2 position)
        {
            return new Vector3(position.x, position.y, 0);
        }

        public Vector2Int FindNearestIndexForPosition(Vector2 position)
        {
            return IndexUnwrap(grid.WorldToCell(PositionWrap(position)));
        }
    }
}