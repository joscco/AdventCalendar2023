using UnityEngine;

public class Game : MonoBehaviour
{
    const string KEY_UNLOCKED_LEVELS = "unlockedLevels";
    
    public static Game instance;
    private int unlockedLevels = 0;
    public const int AVAILABLE_LEVELS = 13;
    
    void Start()
    {
        instance = this;
        unlockedLevels = PlayerPrefs.GetInt(KEY_UNLOCKED_LEVELS, 0);
    }

    // Update is called once per frame
    public void SaveUnlockedLevel(int newLevel)
    {
        if (newLevel > unlockedLevels)
        {
            unlockedLevels = newLevel;
            PlayerPrefs.SetInt(KEY_UNLOCKED_LEVELS, unlockedLevels);
        }
    }

    public int GetUnlockedLevel()
    {
        return unlockedLevels;
    }
}
