using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class StartButton : MonoBehaviour
{
    public SceneTransition sceneTransition;
    public String mainGameSceneName = "GameScene";

    public const float ScaleTimeInSeconds = 0.5f;
    public const float MaxScale = 1.2f;

    private void OnMouseEnter()
    {
        transform.DOScale(MaxScale, ScaleTimeInSeconds).SetEase(Ease.OutBack);
    }

    private void OnMouseExit()
    {
        transform.DOScale(1f, ScaleTimeInSeconds).SetEase(Ease.OutBack);
    }

    private void OnMouseUp()
    {
        sceneTransition.TransitionTo(mainGameSceneName);
    }
}
