using DG.Tweening;
using GameScene.PlayerControl;
using UnityEngine;

public class Vortex : MovableGridEntity
{
    private Tween moveTween;
    private Tween rotateTween;
    
    void Start()
    {
        moveTween = transform.DOLocalMoveY(-20, 2)
            .SetEase(Ease.InOutQuad)
            .SetLoops(-1, LoopType.Yoyo);
        rotateTween = transform.DOLocalRotate(new Vector3(0, 0, -360), 12, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }
}
