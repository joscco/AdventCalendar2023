using GameScene.PlayerControl;

namespace Levels.WizardLevel
{
    public class WizardVioletTile : MovableGridEntity
    {
        public void SetSortOrder(int order)
        {
            spriteRenderer.sortingOrder = order;
        }
    }
}