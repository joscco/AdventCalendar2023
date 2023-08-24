using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameScene.Grid.Managers
{
    public class TilemapManager : MonoBehaviour
    {
        private Vector2Int _tileBoundsMin = new(-500, -500);
        private Vector2Int _tileBoundsMax = new(500, 500);
        private List<Vector2Int> indices = new List<Vector2Int>();

        [SerializeField] private Tilemap tilemap;
        [SerializeField] private TilemapRenderer tileRenderer;

        public bool HasTileAt(Vector2Int index)
        {
            return tilemap.HasTile(IndexWrap(index));
        }
        
        public TileBase GetAt(Vector2Int index)
        {
            return tilemap.GetTile(IndexWrap(index));
        }
        
        public Vector3Int IndexWrap(Vector2Int index)
        {
            return new Vector3Int(index.x, index.y, 0);
        }

        public List<Vector2Int> GetIndices()
        {
            if (indices.Count == 0)
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

                indices = result;
            }
            
            return indices;
        }

        public void Hide()
        {
            tileRenderer.enabled = false;
        }
    }
}