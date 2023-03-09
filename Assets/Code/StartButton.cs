using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class StartButton : MonoBehaviour
{
    [FormerlySerializedAs("_levelLoader")] [SerializeField]
    private SceneTransition sceneTransition;
    public String mainGameSceneName = "GameScene";
    public float scaleTimeInSeconds = 0.5f;
    public float maxScale = 1.2f;
    private void OnMouseEnter()
    {
        transform.DOScale(maxScale, scaleTimeInSeconds).SetEase(Ease.OutBack);
    }

    private void OnMouseExit()
    {
        transform.DOScale(1f, scaleTimeInSeconds).SetEase(Ease.OutBack);
    }

    private void OnMouseUp()
    {
        sceneTransition.TransitionTo(mainGameSceneName);
    }
}
