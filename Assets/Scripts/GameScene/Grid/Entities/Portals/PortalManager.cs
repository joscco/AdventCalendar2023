using System;
using System.Collections.Generic;
using System.Linq;
using GameScene.Grid.Managers;
using SceneManagement;
using UnityEngine;

namespace GameScene.SpecialGridEntities.EntityManagers
{
    public class PortalManager : GridEntityManager<Portal>
    {
        [SerializeField] private List<PortalCycle> cycles;

        public Vector2Int FindNextPortalIndexFor(Vector2Int index)
        {
            return cycles.First(cycle => cycle.HasAt(index))
                .FindNextPortalIndex(index);
        }
        
        [Serializable]
        public class PortalCycle
        {
            [SerializeField] private List<Portal> portals;

            public Vector2Int FindNextPortalIndex(Vector2Int index)
            {
                var portal = portals.First(portal => portal.GetMainIndex() == index);
                return FindNext(portal).GetMainIndex();
            }

            private Portal FindNext(Portal portal)
            {
                var index = portals.IndexOf(portal);
                return portals[(index + 1) % portals.Count];
            }

            public bool HasAt(Vector2Int index)
            {
                return portals.Any(portal => portal.GetMainIndex() == index);
            }
        }
    }
}