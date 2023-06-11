using System.Collections.Generic;
using SceneManagement;
using UnityEngine;

namespace LevelChoosingScene
{
    [CreateAssetMenu(menuName = "AllLevelsConfig")]
    public class LevelConfig : ScriptableObject
    {
        public List<SingleLevelConfig> levels;
    }
}