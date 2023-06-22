using Code.GameScene.UI;
using DG.Tweening;
using SceneManagement;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelButton : ScalingButton
{
    [SerializeField] private Sprite frontSprite;
    [SerializeField] private Sprite backSprite;
    [SerializeField] private LevelButtonRenderer buttonRenderer;

    private Sequence _turnTween;
    private bool turnedToFront;
    private SceneReference _levelName;

    public void SetFrontSprite(Sprite sprite)
    {
        frontSprite = sprite;
        buttonRenderer.UpdateSpriteRenderer(turnedToFront ? frontSprite : backSprite);
    }

    public void TurnOn()
    {
        buttonRenderer.TurnTo(frontSprite);
    }

    public void TurnOff()
    {
        buttonRenderer.TurnTo(backSprite);
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
