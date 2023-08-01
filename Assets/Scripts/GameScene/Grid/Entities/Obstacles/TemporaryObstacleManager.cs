using System.Collections.Generic;
using System.Linq;
using GameScene.Grid.Entities.Shared;
using GameScene.Grid.Managers;
using UnityEngine;

namespace GameScene.Grid.Entities.Obstacles
{
    public class TemporaryObstacleManager : GridEntityManager<TemporaryObstacle>
    {
        public override HashSet<Vector2Int> GetCoveredIndices()
        {
            return entities.Where(entity => entity.IsBlocking())
                .SelectMany(entity => entity.GetCoveredIndices())
                .ToHashSet();
        }

        public void CheckStatuses()
        {
           entities.ForEach(entity => entity.CheckStatus());
        }
    }
}