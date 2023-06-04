using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Levels.SheepLevel
{
    public class TilemapManager : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;

        public bool HasTileAt(Vector2Int index)
        {
            return tilemap.HasTile(IndexWrap(index));
        }

        public Vector3Int IndexWrap(Vector2Int index)
        {
            return new Vector3Int(index.x, index.y, 0);
        }

        public List<Vector2Int> GetNeighboringIndices(Vector2Int index)
        {
            List<Vector2Int> rawList = new List<Vector2Int>
            {
                new (index.x - 1, index.y - 1),
                new (index.x, index.y - 1),
                new (index.x + 1, index.y - 1),
                new (index.x - 1, index.y),
                new (index.x + 1, index.y),
                new (index.x - 1, index.y + 1),
                new (index.x, index.y + 1),
                new (index.x + 1, index.y + 1)
            };

            return rawList.Where(entry => HasTileAt(entry)).ToList();
        }

        public List<Vector2Int> GetHVNeighboringIndices(Vector2Int index)
        {
            List<Vector2Int> rawList = new List<Vector2Int>
            {
                new (index.x, index.y - 1),
                new (index.x - 1, index.y),
                new (index.x + 1, index.y),
                new (index.x, index.y + 1),
            };

            return rawList.Where(entry => HasTileAt(entry)).ToList();
        }
    }
}