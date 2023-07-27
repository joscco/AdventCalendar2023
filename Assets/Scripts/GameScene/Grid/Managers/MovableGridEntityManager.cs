using GameScene.Grid.Entities.Shared;
using GameScene.Grid.Managers;
using SceneManagement;
using UnityEngine;

namespace General.Grid
{
    public class MovableGridEntityManager<T> : GridEntityManager<T> where T : MovableGridEntity
    {
    }
}