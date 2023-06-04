using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Levels.SheepLevel
{
    public class TilemapManager : MonoBehaviour
    {
        private Vector2Int _tileBoundsMin = new(-10, -10);
        private Vector2Int _tileBoundsMax = new(10, 10);
        
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

        public List<Vector2Int> GetIndices()
        {
            List<Vector2Int> result = new List<Vector2Int>();

            for (int x = _tileBoundsMin.x; x <= _tileBoundsMax.x; x++) {
                for (int y = _tileBoundsMin.y; y <= _tileBoundsMax.y; y++)
                {
                    var index = new Vector2Int(x, y);
                    if (HasTileAt(index)) {
                        result.Add(index);
                    }
                }
            }

            return result;
        }
    }
}