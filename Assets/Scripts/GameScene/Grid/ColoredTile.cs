using GameScene.Grid.Entities.Shared;
using UnityEngine;
using UnityEngine.Rendering;

namespace GameScene.Grid
{
    public class ColoredTile : GridEntity
    {
        [SerializeField] private TileColorScheme scheme;
        [SerializeField] private SpriteRenderer frame;
        [SerializeField] private SpriteRenderer spriteBase;
        [SerializeField] private SortingGroup group;


        public void SetScheme(TileColorScheme scheme, int size)
        {
            this.scheme = scheme;
            if (scheme)
            {
                frame.color =  scheme.frame.value;
                spriteBase.color =  scheme.main.value;
                group.sortingOrder = size + 1;
                frame.size = spriteBase.size =  180 * (0.98f - size * 0.25f) * Vector2.one;
            }
        }
    }
}