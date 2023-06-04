using System;
using System.Collections.Generic;
using System.Linq;
using GameScene.PlayerControl;
using Unity.VisualScripting;
using UnityEngine;

namespace Levels.SheepLevel
{
    public class SheepMoveCalculator
    {
        public SheepMovingResult CalculateNewPositions(
            Vector2Int currentPlayerPosition,
            Vector2Int nextPlayerPosition,
            List<Vector2Int> possibleIndices,
            List<MovableGridEntity> sheep)
        {
            var possibleFieldsForSheep = possibleIndices
                .Where(index => index != currentPlayerPosition)
                .Where(index => index != nextPlayerPosition)
                .Where(index => !GetHVNeighboringIndices(nextPlayerPosition, possibleIndices).Contains(index))
                .ToList();

            var neighboringIndices = GetNeighboringIndices(nextPlayerPosition, possibleIndices);
            List<MovableGridEntity> sheepThatMustMove = sheep
                .Where(singleSheep => neighboringIndices.Contains(singleSheep.GetIndex()))
                .ToList();

            if (sheepThatMustMove.Count == 0)
            {
                return new SheepMovingResult(true, new Dictionary<MovableGridEntity, Vector2Int>());
            }

            return CalculateNewPositionsFor(
                sheepThatMustMove,
                nextPlayerPosition,
                nextPlayerPosition - currentPlayerPosition,
                possibleFieldsForSheep,
                sheep
            );
        }

        private SheepMovingResult CalculateNewPositionsFor(
            List<MovableGridEntity> sheepThatMustMove,
            Vector2Int nextPlayerPosition,
            Vector2Int preferredDirection,
            List<Vector2Int> possibleFieldsForSheepOld,
            List<MovableGridEntity> sheepOld)
        {
            foreach (var firstSheepToMove in sheepThatMustMove)
            {
                Dictionary<MovableGridEntity, Vector2Int> newSheepPositions =
                    new Dictionary<MovableGridEntity, Vector2Int>();
                List<MovableGridEntity> sheepThatCanStillMove = new List<MovableGridEntity>(sheepOld);
                List<Vector2Int> possibleFieldsForSheep = new List<Vector2Int>(possibleFieldsForSheepOld);

                // List And Dict Are Changed!
                var moveResult = TryMoveSheep(
                    firstSheepToMove,
                    sheepThatCanStillMove,
                    possibleFieldsForSheep,
                    preferredDirection
                );

                if (!moveResult.movingPossible)
                {
                    // This try leads to nothing, try next
                    Debug.Log("Moving this sheep first failed");
                    continue;
                }

                var newSheepThatMustMove = new List<MovableGridEntity>(sheepThatMustMove);
                foreach (var movedSheepPair in moveResult.newPosition)
                {
                    var movedSheep = movedSheepPair.Key;
                    var newIndex = movedSheepPair.Value;
                    if (sheepThatMustMove.Contains(movedSheep))
                    {
                        newSheepThatMustMove.Remove(movedSheep);
                    }

                    sheepThatCanStillMove.Remove(movedSheep);
                    possibleFieldsForSheep.Remove(newIndex);
                }

                newSheepPositions.AddRange(moveResult.newPosition);

                if (newSheepThatMustMove.Count > 0)
                {
                    var furtherMoveResult = CalculateNewPositionsFor(
                        newSheepThatMustMove,
                        nextPlayerPosition,
                        preferredDirection,
                        possibleFieldsForSheep,
                        sheepThatCanStillMove);

                    if (!furtherMoveResult.movingPossible)
                    {
                        Debug.Log("Moving first worked but then not");
                        continue;
                    }

                    newSheepPositions.AddRange(furtherMoveResult.newSheepPositions);
                }

                return new SheepMovingResult(true, newSheepPositions);
            }

            Debug.Log("No Moving was Successful");
            return new SheepMovingResult(false, null);
        }

        private SingleSheepMoveResult TryMoveSheep(
            MovableGridEntity sheep,
            List<MovableGridEntity> sheepThatCanStillMove,
            List<Vector2Int> possibleFieldsForSheep,
            Vector2Int preferredDirection)
        {
            var bestNeighbor = new Vector2Int();
            if (possibleFieldsForSheep.Contains(sheep.GetIndex() + preferredDirection))
            {
                bestNeighbor = sheep.GetIndex() + preferredDirection;
            } else if (possibleFieldsForSheep.Contains(sheep.GetIndex() + Perpendiculate(preferredDirection)))
            {
                bestNeighbor = sheep.GetIndex() + Perpendiculate(preferredDirection);
            } else if (possibleFieldsForSheep.Contains(sheep.GetIndex() - Perpendiculate(preferredDirection)))
            {
                bestNeighbor = sheep.GetIndex() - Perpendiculate(preferredDirection);
            } else if (possibleFieldsForSheep.Contains(sheep.GetIndex() - preferredDirection))
            {
                bestNeighbor = sheep.GetIndex() - preferredDirection;
            }
            else
            {
                return new SingleSheepMoveResult(false, null);
            }
            
            var concurringSheep = sheepThatCanStillMove
                .FirstOrDefault(sheep => bestNeighbor == sheep.GetIndex());

            if (null != concurringSheep)
            {
                // Move neighbor sheep
                var newMovableList = new List<MovableGridEntity>(sheepThatCanStillMove);
                newMovableList.Remove(sheep);
                var newPossibleFields = new List<Vector2Int>(possibleFieldsForSheep);
                newPossibleFields.Remove(bestNeighbor);
                var neighborMoveResult = TryMoveSheep(concurringSheep, newMovableList, newPossibleFields, bestNeighbor - sheep.GetIndex());

                // If that fails, everything fails 
                if (!neighborMoveResult.movingPossible)
                {
                    return new SingleSheepMoveResult(false, null);
                }

                // If not gather new Positions from there
                var newPositions = neighborMoveResult.newPosition;
                newPositions.Add(sheep, bestNeighbor);
                return new SingleSheepMoveResult(true, newPositions);
            }

            return new SingleSheepMoveResult(true,
                new Dictionary<MovableGridEntity, Vector2Int> { { sheep, bestNeighbor } });
        }

        private Vector2Int Perpendiculate(Vector2Int preferredDirection)
        {
            return new Vector2Int(preferredDirection.y, preferredDirection.x);
        }

        private int GetIndexDistance(Vector2Int index1, Vector2Int index2)
        {
            return Math.Abs(index1.x - index2.x) + Math.Abs(index1.y - index2.y);
        }

        public List<Vector2Int> GetNeighboringIndices(Vector2Int index, List<Vector2Int> possibleFields)
        {
            List<Vector2Int> rawList = new List<Vector2Int>
            {
                new(index.x - 1, index.y - 1),
                new(index.x, index.y - 1),
                new(index.x + 1, index.y - 1),
                new(index.x - 1, index.y),
                new(index.x + 1, index.y),
                new(index.x - 1, index.y + 1),
                new(index.x, index.y + 1),
                new(index.x + 1, index.y + 1)
            };

            return rawList.Where(entry => possibleFields.Contains(entry)).ToList();
        }

        public List<Vector2Int> GetHVNeighboringIndices(Vector2Int index, List<Vector2Int> possibleFields)
        {
            List<Vector2Int> rawList = new List<Vector2Int>
            {
                new(index.x, index.y - 1),
                new(index.x - 1, index.y),
                new(index.x + 1, index.y),
                new(index.x, index.y + 1),
            };

            return rawList.Where(entry => possibleFields.Contains(entry)).ToList();
        }
    }
}