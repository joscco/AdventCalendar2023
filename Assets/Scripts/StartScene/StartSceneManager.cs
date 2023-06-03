using DG.Tweening;
using GameScene;
using GameScene.UI;
using General;
using StartScene;
using UnityEngine;

public class StartSceneManager : MonoBehaviour
{
    public TitleAnimation titleAnimation;
    public StartButton startButton;

    private void Start()
    {
        //startButton.OnSetActive();
    }
    
    // private void Update()
    // {
    //     var move = InputManager.instance.GetMoveDirection();
    //         
    //     if (OptionScreen.instance.IsVisible())
    //     {
    //         // Option Screen is showing
    //         OptionScreen.instance.HandleMoveInput(move);
    //         if (InputManager.instance.GetEnterOrSpace())
    //         {
    //             OptionScreen.instance.OnPressEnter();
    //         }
    //     }
    //     else
    //     {
    //         if (InputManager.instance.GetEnterOrSpace())
    //         {
    //             startButton.Trigger();
    //         }
    //     }
    //
    //     if (!SceneManager.Get().IsInTransition())
    //     {
    //         if (Input.GetKeyDown(KeyCode.O))
    //         {
    //             FindObjectOfType<OptionButton>()?.Trigger();
    //         }
    //     }
    // }

    public void AfterSceneStart()
    {
        DOVirtual.DelayedCall(0.5f, () => titleAnimation.FadeIn());
    }
}
