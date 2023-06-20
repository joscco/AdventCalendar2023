using General;
using SceneManagement;
using UnityEngine;

public class FocusScene : MonoBehaviour
{
    private void OnMouseUp()
    {
        SceneTransitionManager.Get().TransitionToScene(SceneReference.START);
        AudioManager.instance.PlayMusic();
    }
}
