using UnityEngine;

namespace GameScene
{
    public abstract class LevelManager : MonoBehaviour
    {
        public abstract void HandleUpdate();

        public abstract bool HasWon();

        public abstract bool HasLost();
    }
}