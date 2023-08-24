using GameScene.Grid.Managers;
using UnityEngine;

namespace GameScene.Grid.Entities.ToggleableTile
{
    public class ToggleableTileManager : GridEntityManager<Levels.WizardLevel.ToggleableTile>
    {

        public bool HasActiveAt(Vector2Int vector2Int)
        {
            if (HasAt(vector2Int))
            {
                var tile = GetAt(vector2Int);
                return tile.IsActive();
            }

            return false;
        }
    }
}