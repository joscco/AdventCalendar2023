using GameScene.PlayerControl;

namespace Levels.WizardLevel
{
    public class WizardRealTile : MovableGridEntity
    {
        public void SetSortOrder(int order)
        {
            spriteRenderer.sortingOrder = order;
        }
    }
}