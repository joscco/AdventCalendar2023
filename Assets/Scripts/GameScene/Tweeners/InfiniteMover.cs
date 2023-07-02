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
        var currentY = transform.position.y;
        moveTween = transform.DOMoveY(currentY + offsetY, duration)
            .SetEase(Ease.InOutQuad)
            .SetLoops(-1, LoopType.Yoyo);
    }
}