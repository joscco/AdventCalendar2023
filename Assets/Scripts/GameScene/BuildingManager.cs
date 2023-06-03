using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameScene.Buildings
{
    public abstract class BuildingManager<T> : MonoBehaviour where T : Building
    {
        [SerializeField] private Grid grid;
        private List<T> _buildings;

        private void Start()
        {
            _buildings = new();
        }

        public void AddBuildingAt(T building, Vector2Int index)
        {
            building.SetIndex(index);
            building.transform.position = IndexToPosition(building.GetIndex());
            _buildings.Add(building);
        }

        public bool HasBuildingAt(Vector2Int index)
        {
            return _buildings.Any(building => building.GetIndex() == index);
        }

        public Vector2 IndexToPosition(Vector2Int index)
        {
            return grid.GetCellCenterWorld(IndexWrap(index));
        }

        public Vector3Int IndexWrap(Vector2Int index)
        {
            return new Vector3Int(index.x, index.y, 0);
        }

        public T GetBuildingAt(Vector2Int index)
        {
            return _buildings.First(building => building.GetIndex() == index);
        }

        public List<T> GetBuildings()
        {
            return _buildings;
        }
    }
}