using UI;
using UnityEngine;

public class LevelButton : ScalingButton
{
    [SerializeField] private LevelButtonBase baseButton;
    [SerializeField] private Sprite image;
    [SerializeField] private int level;

    public const float Width = 600f;

    private void Start()
    {
        baseButton.Sort((level - 1) % 4 + 1);
        baseButton.SetSprite(image);
        transform.localPosition = Vector2.right * (level - 1) * Width;
    }

    public void TurnOn()
    {
        baseButton.TurnOn();
    }

    public void TurnOff()
    {
        baseButton.TurnOff();
    }

    public int GetLevel()
    {
        return level;
    }
}