using UnityEngine;

public class MonoColoredTile : ColoredTile
{
    [SerializeField] private PaletteColor color;

    [SerializeField] private SpriteRenderer spriteBase;

    private void OnValidate()
    {
        frame.color = frameColor.value;
        back.color = backColor.value;
        spriteBase.color = color.value;
    }
}
