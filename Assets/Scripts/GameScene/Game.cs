using System;
using UnityEngine;

namespace GameScene
{
    public class Game : MonoBehaviour
    {
        const string KeyUnlockedLevels = "unlockedLevels";
    
        public static Game instance;
        private int _unlockedLevels = 1;
        public const int AvailableLevels = 3;
    
        void Start()
        {
            instance = this;
            _unlockedLevels = Math.Min(PlayerPrefs.GetInt(KeyUnlockedLevels, 1), AvailableLevels);
        }

        // Update is called once per frame
        public void SaveUnlockedLevel(int newLevel)
        {
            if (newLevel > _unlockedLevels)
            {
                _unlockedLevels = Math.Min(newLevel, AvailableLevels);
                PlayerPrefs.SetInt(KeyUnlockedLevels, _unlockedLevels);
            }
        }

        public int GetUnlockedLevels()
        {
            return _unlockedLevels;
        }

        public void SavePreferredLanguage(LanguageIdentifier identifier)
        {
            PlayerPrefs.SetString("preferredLanguage", identifier.ToString());
        }

        public LanguageIdentifier GetPreferredLanguage()
        {
            var savedValue = PlayerPrefs.GetString("preferredLanguage", "En");
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
    
        public static void SaveSfxVolume(float newVolume)
        {
            PlayerPrefs.SetFloat("sfxLevel", newVolume);
        }

        public static float GetSfxVolume()
        {
            return PlayerPrefs.GetFloat("sfxLevel", 0.5f);
        }

        public enum LanguageIdentifier
        {
            En, De
        }
    }
}
