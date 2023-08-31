using UI;
using UnityEngine;
using UnityEngine.Localization;

public class LevelButton : ScalingButton
{
    [SerializeField] private LevelButtonBase baseButton;
    [SerializeField] private int level;
    [SerializeField] private LocalizedString text;

    public const float Width = 750f;

    private void Start()
    {
        transform.localScale = 0.9f * Vector3.one;
        baseButton.SetNumber(level);
        baseButton.SetText(text);
        transform.localPosition = Vector2.right * (level - 1) * Width;
    }

    protected override float GetScaleWhenSelected()
    {
        return 1.1f;
    }
    
    protected override float GetScaleWhenNeutral()
    {
        return 0.9f;
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