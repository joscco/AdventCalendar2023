using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(SpriteRenderer))]
public class LevelButtonRenderer: MonoBehaviour
{
    [FormerlySerializedAs("_spriteRenderer")] [SerializeField] private SpriteRenderer spriteRenderer;

    private Sequence _turnTween;

    public void UpdateSpriteRenderer(Sprite newSprite)
    {
        spriteRenderer.sprite = newSprite;
    }

    public void TurnTo(Sprite newSprite)
    {
        _turnTween?.Kill();
        _turnTween = DOTween.Sequence();
        _turnTween.Append(Rotate(0))
            .AppendCallback(() =>
            {
                UpdateSpriteRenderer(newSprite);
            })
            .Append(Rotate(1));
    }

    private Tween Rotate(float val)
    {
        return spriteRenderer.transform
            .DOScaleX(val, 0.1f)
            .SetEase(Ease.InOutCubic);
    }
}