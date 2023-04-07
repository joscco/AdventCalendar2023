using UnityEngine;

namespace LevelChooserScene
{
    [CreateAssetMenu(fileName = "New LevelCardData", menuName = "LevelCardData")]
    public class LevelCardData : ScriptableObject
    {
        public Sprite sprite;
        public string levelName;
        public int level;
    }
}