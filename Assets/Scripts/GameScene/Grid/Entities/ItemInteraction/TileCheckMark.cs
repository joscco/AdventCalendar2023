using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TileCheckMark : MonoBehaviour
{
    [SerializeField] private SpriteRenderer checkRenderer;

    private Tween scaleTween;

    public void Hide()
    {
        scaleTween = checkRenderer.transform.DOScale(0, 0.3f)
            .SetEase(Ease.InOutQuad);
    }
    
    public void Show()
    {
        scaleTween = checkRenderer.transform.DOScale(1, 0.3f)
            .SetEase(Ease.InOutQuad);
    }
}