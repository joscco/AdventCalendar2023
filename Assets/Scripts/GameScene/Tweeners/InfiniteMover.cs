using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class InfiniteMover : MonoBehaviour
{
    [SerializeField] private float offsetY = -20f;
    [SerializeField] private float duration = 2f;
    private Tween moveTween;

    void Start()
    {
        var currentY = transform.localPosition.y;
        moveTween = transform.DOLocalMoveY(currentY + offsetY, duration)
            .SetEase(Ease.InOutQuad)
            .SetLoops(-1, LoopType.Yoyo);
    }
}