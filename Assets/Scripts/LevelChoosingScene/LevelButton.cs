using Code.GameScene.UI;
using DG.Tweening;
using SceneManagement;
using UnityEngine;

public class LevelButton : ScalingButton
{
    [SerializeField] private Sprite frontSprite;
    [SerializeField] private Sprite backSprite;
    [SerializeField] private LevelButtonRenderer renderer;

    private Sequence _turnTween;
    private bool turnedToFront;
    private SceneReference _levelName;

    public void SetFrontSprite(Sprite sprite)
    {
        frontSprite = sprite;
        renderer.UpdateSpriteRenderer(turnedToFront ? frontSprite : backSprite);
    }

    public void TurnOn()
    {
        renderer.TurnTo(frontSprite);
    }

    public void TurnOff()
    {
        renderer.TurnTo(backSprite);
    }
    
    public override void OnClick()
    {
        if (null != _levelName)
        {
            SceneTransitionManager.Get().TransitionToScene(_levelName);
        }
    }

    public override bool IsEnabled()
    {
        return true;
    }
    
    public override void Select()
    {
        _selected = true;
        TurnOn();
    }

    public override void Deselect()
    {
        _selected = false;
        TurnOff();
    }

    public void SetLevelName(SceneReference levelSceneName)
    {
        _levelName = levelSceneName;
    }
}
