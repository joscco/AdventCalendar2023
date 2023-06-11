using SceneManagement;
using UnityEngine;

namespace LevelChoosingScene
{
    [CreateAssetMenu(menuName = "LevelConfig")]
    public class SingleLevelConfig: ScriptableObject
    {
        public Sprite levelIcon;
        public LevelReference levelName;
    }
}