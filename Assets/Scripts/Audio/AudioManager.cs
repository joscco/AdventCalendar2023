using DG.Tweening;
using UnityEngine;

namespace General
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;
        
        public AudioSource EffectsSource;
        public AudioSource MusicSource;

        public AudioClip mainMusic;
        public AudioClip[] blubSounds;
        public AudioClip[] moveSounds;

        private void Start()
        {
            instance = this;
            SetMusicVolume(PlayerPrefs.GetFloat("musicLevel", 1f));
            SetSFXVolume(PlayerPrefs.GetFloat("sfxLevel", 1f));
        }

        public void SetMusicVolume(float value)
        {
            float oldValue = MusicSource.volume;
            MusicSource.volume = value;
            PlayerPrefs.SetFloat("musicLevel", value);
            
            if (oldValue <= 0.05 && value > 0.05)
            {
                PlayMusic();
            } else if (value <= 0.05)
            {
                StopMusic();
            }
        }
        
        public void SetSFXVolume(float value)
        {
            EffectsSource.volume = value;
            PlayerPrefs.SetFloat("sfxLevel", value);
        }
        
        public void PlayBlub()
        {
            EffectsSource.clip = blubSounds[Random.Range(0, blubSounds.Length)];
            EffectsSource.Play();
        }

        public void PlayMoveSound()
        {
            EffectsSource.clip = moveSounds[Random.Range(0, moveSounds.Length)];
            EffectsSource.Play();
        }

        // Play a single clip through the music source.
        public void PlayMusic()
        {
            if (!MusicSource.isPlaying)
            {
                MusicSource.clip = mainMusic;
                MusicSource.Play();
            }
        }
        
        public void StopMusic()
        {
            MusicSource.Stop();
        }

        public float GetMusicVolume()
        {
            return MusicSource.volume;
        }

        public float GetSFXVolume()
        {
            return EffectsSource.volume;
        }
    }
}