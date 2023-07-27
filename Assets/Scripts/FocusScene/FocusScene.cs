using General;
using SceneManagement;
using UnityEngine;

public class FocusScene : MonoBehaviour
{
    private void OnMouseUp()
    {
        SceneTransitionManager.Get().TransitionToNonLevelScene("StartScene");
        AudioManager.instance.PlayMusic();
    }
}
