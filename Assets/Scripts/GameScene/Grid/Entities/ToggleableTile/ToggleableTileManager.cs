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
        public List<Vector2Int> GetActiveTileIndices()
        {
            return GetEntities()
                .Where(tile => tile.IsActive())
                .Select(tile => tile.GetMainIndex())
                .ToList();
        }
    }
}