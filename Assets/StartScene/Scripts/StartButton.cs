using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    [SerializeField]
    private LevelLoader _levelLoader;
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
        _levelLoader.LoadLevel(mainGameSceneName);
    }
}
