using System.Collections.Generic;
using GameScene.PlayerControl;
using UnityEngine;

namespace Levels.SheepLevel
{
    public struct SheepMovingResult
    {
        public bool movingPossible;
        public Dictionary<MovableGridEntity, Vector2Int> newSheepPositions;

        public SheepMovingResult(bool movingPossible, Dictionary<MovableGridEntity, Vector2Int> newSheepPositions)
        {
            this.movingPossible = movingPossible;
            this.newSheepPositions = newSheepPositions;
        }
    }

    public struct SingleSheepMoveResult
    {
        public bool movingPossible;
        public Dictionary<MovableGridEntity, Vector2Int> newPosition;

        public SingleSheepMoveResult(bool movingPossible, Dictionary<MovableGridEntity, Vector2Int> newPosition)
        {
            this.movingPossible = movingPossible;
            this.newPosition = newPosition;
        }
    }
}