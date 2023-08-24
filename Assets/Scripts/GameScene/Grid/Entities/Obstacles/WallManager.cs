using System.Collections.Generic;
using System.Linq;
using GameScene.Facts;
using GameScene.Grid.Managers;
using UnityEngine;

namespace GameScene.Grid.Entities.Obstacles
{
    public class WallManager : GridEntityManager<Wall>
    {
        public bool HasBlockedAt(Vector2Int index)
        {
            return entities
                .Where(entity => entity.GetMainIndex() == index)
                .Any(entity => entity.IsBlocking());
        }
    }
}