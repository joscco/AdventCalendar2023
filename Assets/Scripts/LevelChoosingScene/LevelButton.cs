using Code.GameScene.UI;
using SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class LevelButton : ScalingButton
{
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private SortingGroup sortingGroup;
    [SerializeField] private SpriteRenderer boxRenderer;
    [SerializeField] private SpriteRenderer numberRenderer;
    [SerializeField] private int level;
    [SerializeField] private bool _active;

    public void SetFrontSprite(Sprite sprite)
    {
        activeSprite = sprite;
        if (_active)
        {
            TurnOn();
        }
        else
        {
            TurnOff();
        }
    }

    public void TurnOn()
    {
        sortingGroup.sortingOrder = (level - 1) % 4 + 1;
        numberRenderer.color = new Color(1, 1, 1, 1);
        boxRenderer.color = new Color(1, 1, 1, 1);
        boxRenderer.sprite = activeSprite;
    }

    public void TurnOff()
    {
        sortingGroup.sortingOrder = 0;
        numberRenderer.color = new Color(1, 1, 1, 0.4f);
        boxRenderer.color = new Color(1, 1, 1, 0.5f);
        boxRenderer.sprite = inactiveSprite;
    }

    protected override void OnClick()
    {
        SceneTransitionManager.Get().TransitionToLevel(level);
    }

    protected override bool IsEnabled()
    {
        return true;
    }

    public int GetLevel()
    {
        return level;
    }
}