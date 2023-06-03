using General;
using SceneManagement;
using UnityEngine;

public class EntryScene : MonoBehaviour
{
    private void OnMouseUp()
    {
        SceneTransitionManager.Get().TransitionToScene(SceneReference.START);
        AudioManager.instance.PlayMusic();
    }
}
