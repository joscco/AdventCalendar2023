using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using SceneManagement;
using UnityEngine;

namespace Levels.WizardLevel
{
    public class WizardMirrorTileManager : MovableGridEntityManager<WizardMirrorTile>
    {
        private Tween _updateSequence;

        public void UpdateMirrorTileIndices(HashSet<Vector2Int> indicesForMirrorTiles)
        {
            var toList = indicesForMirrorTiles.ToList();
            _updateSequence?.Kill();
            SetAllInactive();
            SetActive(toList);
            _updateSequence = BlendOutAll().OnComplete(() =>
            {
                InstantMoveToInternalIndex().OnComplete(() =>
                {
                    BlendInActive();
                });
            });
        }

        private Tween BlendOutAll()
        {
            var sequence = DOTween.Sequence();
            foreach (var mirrorTile in _entities)
            {
                sequence.Insert(0, mirrorTile.BlendOut());
            }

            return sequence;
        }
        
        private Tween InstantMoveToInternalIndex()
        {
            var sequence = DOTween.Sequence();
            foreach (var mirrorTile in _entities)
            {
                sequence.InsertCallback(0, () =>
                {
                    var index = mirrorTile.GetStoredIndex();
                    mirrorTile.InstantUpdatePosition(index, IndexToPosition(index));
                    mirrorTile.SetSortOrder(-index.y);
                });
            }

            return sequence;
        }
        
        private Tween BlendInActive()
        {
            var sequence = DOTween.Sequence();
            foreach (var mirrorTile in _entities)
            {
                if (mirrorTile.IsActive())
                {
                    sequence.Insert(0, mirrorTile.BlendIn());
                }
            }

            return sequence;
        }

        private void SetAllInactive()
        {
            foreach (var mirrorTile in _entities)
            {
                mirrorTile.SetActive(false);
            }
        }

        private void SetActive(List<Vector2Int> newActiveIndices)
        {
            for (int i = 0; i < newActiveIndices.Count; i++)
            {
                var index = newActiveIndices[i];
                var mirrorTile = _entities[i];
                mirrorTile.StoreIndex(index);
                mirrorTile.SetActive(true);
            }
        }

        public List<Vector2Int> GetActiveIndices()
        {
            return _entities.Where(tile => tile.IsActive())
                .Select(tile => tile.GetIndex())
                .ToList();
        }
    }
}