using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace GameScene.Grid.Entities.Obstacles
{
    public class Locomotive : TemporaryObstacle
    {
        [SerializeField] private List<Transform> wheels;
        [SerializeField] private Vector3 offsetWhenComplete;
        [SerializeField] private float secondsToMoveOut;

        protected override void OnUnblock()
        {
            wheels.ForEach(wheel => wheel
                .DORotate(new Vector3(0, 0, 360), 1f, RotateMode.FastBeyond360)
                .SetLoops(5));

            transform.DOMove(transform.position + offsetWhenComplete, secondsToMoveOut).SetEase(Ease.InQuad);
        }
    }
}