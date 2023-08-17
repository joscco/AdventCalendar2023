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

        public bool HasCoveredAt(Vector2Int index)
        {
            return entities
                .Where(entity => entity.GetCoveredIndices().Contains(index))
                .Any(entity => entity.IsBlocking());
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