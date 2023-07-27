using UnityEngine;

namespace GameScene.Grid.Entities.Shared
{
    public class TemporaryObstacle : GridEntity
    {
        [SerializeField] private bool blocking = true;

        public bool IsBlocking()
        {
            return blocking;
        }

        public void SetBlocking(bool val)
        {
            this.blocking = val;
        }
    }
}