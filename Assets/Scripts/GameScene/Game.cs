using System;
using UnityEngine;
using UnityEngine.Localization;

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
            unlockedLevels = Math.Min(newLevel, AVAILABLE_LEVELS);
            PlayerPrefs.SetInt(KEY_UNLOCKED_LEVELS, unlockedLevels);
        }
    }

    public int GetUnlockedLevels()
    {
        return unlockedLevels;
    }

    public void SavePreferredLanguage(LanguageIdentifier identifier)
    {
        PlayerPrefs.SetString("preferredLanguage", identifier.ToString());
    }

    public LanguageIdentifier GetPreferredLanguage()
    {
        var savedValue = PlayerPrefs.GetString("preferredLanguage", "EN");
        return (LanguageIdentifier) Enum.Parse(typeof(LanguageIdentifier), savedValue); 
    }

    public void SaveMusicVolume(float newVolume)
    {
        PlayerPrefs.SetFloat("musicLevel", newVolume);
    }

    public float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat("musicLevel", 0.5f);
    }
    
    public void SaveSFXVolume(float newVolume)
    {
        PlayerPrefs.SetFloat("sfxLevel", newVolume);
    }

    public float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat("sfxLevel", 0.5f);
    }

    public enum LanguageIdentifier
    {
        EN, DE
    }
}
