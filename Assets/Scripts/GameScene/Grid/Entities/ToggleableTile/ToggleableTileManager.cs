using System.Collections.Generic;
using System.Linq;
using GameScene.Grid.Managers;
using Levels.WizardLevel;
using SceneManagement;
using UnityEngine;

namespace GameScene.SpecialGridEntities.EntityManagers
{
    public class ToggleableTileManager : GridEntityManager<ToggleableTile>
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