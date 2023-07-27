using System;
using UnityEngine;

public class Game : MonoBehaviour
{
    const string KEY_UNLOCKED_LEVELS = "unlockedLevels";
    
    public static Game instance;
    private int unlockedLevels = 1;
    public const int AVAILABLE_LEVELS = 24;
    
    void Start()
    {
        instance = this;
        unlockedLevels = PlayerPrefs.GetInt(KEY_UNLOCKED_LEVELS, 1);
    }

    // Update is called once per frame
    public void SaveUnlockedLevel(int newLevel)
    {
        if (newLevel > unlockedLevels)
        {
            unlockedLevels = Math.Max(newLevel, AVAILABLE_LEVELS);
            PlayerPrefs.SetInt(KEY_UNLOCKED_LEVELS, unlockedLevels);
        }
    }

    public int GetHighestUnlockedLevel()
    {
        return unlockedLevels;
    }
}
