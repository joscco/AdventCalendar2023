using DG.Tweening;
using UnityEngine;

public class InfiniteMover : MonoBehaviour
{
    [SerializeField] private float offsetY = -20f;
    [SerializeField] private float duration = 2f;
    private Tween moveTween;
    
    void Start()
    {
        moveTween = transform.DOLocalMoveY(offsetY, duration)
            .SetEase(Ease.InOutQuad)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
