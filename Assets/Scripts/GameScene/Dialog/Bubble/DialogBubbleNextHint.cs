using DG.Tweening;
using UnityEngine;

public class DialogBubbleNextHint : MonoBehaviour
{
    private void Start()
    {
        transform.localScale = Vector3.zero;
    }

    public Tween BlendIn()
    {
        return transform.DOScale(1, 0.3f)
            .SetEase(Ease.InOutQuad);
    }

    public Tween BlendOut()
    {
        return transform.DOScale(0, 0.3f)
            .SetEase(Ease.InOutQuad);
    }
}