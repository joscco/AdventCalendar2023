using UnityEngine;

namespace GameScene
{
    public interface ILevelManager
    {
        public void HandleUpdate();

        public bool HasWon();

        public bool HasLost();
    }
}