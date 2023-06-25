using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class InfiniteTurner : MonoBehaviour
{
    [SerializeField] private float degreesPerSecond = 60f;
    private Tween _rotateTween;

    void Start()
    {
        _rotateTween = transform
            .DORotate(new Vector3(0, 0, degreesPerSecond), 1, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental);
    }
}