using System;
using System.Collections.Generic;
using System.Linq;
using GameScene.Grid.Entities.ItemInteraction;
using GameScene.Grid.Entities.ItemInteraction.Logic.Checkes;
using UnityEngine;

namespace GameScene.Grid.Entities.Obstacles
{
    public class PuzzleArea : MonoBehaviour
    {
        private bool _wasSolved;
        private List<Wall> _wallsInChargeOf;
        private List<GrassFloor> _grassFloorsInChargeOf;
        private List<MultiTagChecker> _checkers;

        public void SetWallsInChargeOf(List<Wall> walls)
        {
            _wallsInChargeOf = walls;
        }
        
        public void SetGrassFloorsInChargeOf(List<GrassFloor> floors)
        {
            _grassFloorsInChargeOf = floors;
        }

        public void SetCheckers(List<MultiTagChecker> checkers)
        {
            this._checkers = checkers;
        }

        public void Check(Dictionary<Vector2Int, InteractableItem> itemMap, Vector2Int playerIndex)
        {
            if (!_wasSolved)
            {
                _checkers.ForEach(entity => entity.Check(itemMap));
                if (_checkers.All(checker => checker.IsSatisfied(itemMap)))
                {
                    UnblockAllWalls();
                    RevealGrassFloors(playerIndex);
                    UncheckAllMyItems(itemMap);
                    _wasSolved = true;
                } 
            }
        }

        private void UnblockAllWalls()
        {
            _wallsInChargeOf.ForEach(wall => wall.Unblock());
        }

        private void RevealGrassFloors(Vector2Int playerIndex)
        {
            _grassFloorsInChargeOf.ForEach(entity =>
            {
                var diffToPlayer = entity.GetMainIndex() - playerIndex;
                var dist = Math.Max(Math.Abs(diffToPlayer.x), Math.Abs(diffToPlayer.y));
                entity.Show(dist * 0.1f);
            });
        }

        private void UncheckAllMyItems(Dictionary<Vector2Int, InteractableItem> itemMap)
        {
            foreach (var checkerIndex in _checkers.Select(checker => checker.GetMainIndex()))
            { 
                itemMap[checkerIndex].Uncheck();
            }
        }
    }
}