using System.Collections.Generic;
using System.Linq;
using GameScene.Facts;
using GameScene.Grid.Managers;
using UnityEngine;

namespace GameScene.Grid.Entities.Obstacles
{
    public class TemporaryObstacleManager : GridEntityManager<TemporaryObstacle>
    {
        [SerializeField] private FactManager factManager;

        private void Start()
        {
            factManager.onNewFacts += UpdateStatuses;
        }

        private void OnDestroy()
        {
            factManager.onNewFacts -= UpdateStatuses;
        }

        public HashSet<Vector2Int> GetCoveredIndices()
        {
            return Enumerable.ToHashSet(entities.Where(entity => entity.IsBlocking())
                .SelectMany(entity => entity.GetCoveredIndices()));
        }

        private void UpdateStatuses(List<Fact> newFacts)
        {
            entities.ForEach(entity =>
            {
                if (entity.IsBlocking())
                {
                    if (factManager.ConditionsAreMet(entity.GetConditionsForCompletion()))
                    {
                        entity.Unblock();
                    }
                }
            });
        }
    }
}