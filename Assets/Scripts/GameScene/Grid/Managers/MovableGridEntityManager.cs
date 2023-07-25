using GameScene.Grid.Managers;
using GameScene.PlayerControl;
using SceneManagement;
using UnityEngine;

namespace General.Grid
{
    public class MovableGridEntityManager<T> : GridEntityManager<T> where T : MovableGridEntity
    {
    }
}