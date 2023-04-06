using UnityEngine;

namespace Code.LevelChooserScene
{
    [CreateAssetMenu(fileName = "New LevelCardData", menuName = "LevelCardData")]
    public class LevelCardData : ScriptableObject
    {
        public Sprite sprite;
        public string levelName;
        public int level;
    }
}