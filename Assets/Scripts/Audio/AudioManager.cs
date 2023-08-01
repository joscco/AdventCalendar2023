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
        }

        public void SetMusicVolume(float value)
        {
            MusicSource.volume = value;
            Game.instance.SaveMusicVolume(value);

            if (!MusicSource.isPlaying && value > 0.05)
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
            Game.instance.SaveSFXVolume(value);
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
    }
}