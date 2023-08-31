using GameScene.Grid.Managers;
using UnityEngine;

namespace GameScene.Grid.Entities.ItemInteraction
{
    public class InteractableItemManager : GridEntityManager<WordTile>
    {
        public void AddAtAndMoveTo(WordTile entity, Vector2Int index, Vector2 position, bool emitParticles = false)
        {
            entity.RelativeMoveTo(index, position, emitParticles);
            entities.Add(entity);
        }
    }
}